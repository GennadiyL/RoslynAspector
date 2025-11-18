using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.MSBuild;
using RoslynAspector.TotalLoggingData;
using Project = Microsoft.CodeAnalysis.Project;

namespace RoslynAspector.TotalLogging;

internal class Program
{
	public static async Task Main(string[] args)
	{
		string solutionPath = GetSolutionPath(args);

		MSBuildWorkspace workspace = CreateMsBuildWorkspace();

		Solution solution = await OpenSolution(workspace, solutionPath);

		solution = await ProcessProjects(solution);

		Console.WriteLine($"Update the Solution {DateTime.Now: HH:mm:ss.fff}");
		workspace.TryApplyChanges(solution);
		Console.WriteLine($"Solution updating is completed {DateTime.Now: HH:mm:ss.fff}");
	}

	private static string GetSolutionPath(string[] args)
	{
		//return @"s:\\Gena\Local\Work\_Drive\Projects\Learning\Programming\Aop\Git\RoslynAspector\RoslynAspectorDemo\RoslynAspectorDemo.sln";
		if (args.Length < 1)
		{
			throw new ArgumentException("\"First argument must be a path to the solution.\"");
		}

		string solutionPath = args[0];
		if (!File.Exists(solutionPath) || Path.GetExtension(solutionPath) != ".sln")
		{
			throw new ArgumentException($"Solution file not found: {solutionPath}");
		}

		return solutionPath;
	}

	private static MSBuildWorkspace CreateMsBuildWorkspace()
	{
		MSBuildLocator.RegisterDefaults();
		MSBuildWorkspace workspace = MSBuildWorkspace.Create();
		return workspace;
	}

	private static async Task<Solution> OpenSolution(MSBuildWorkspace workspace, string solutionPath)
	{
		Console.WriteLine($"Open the Solution {DateTime.Now: HH:mm:ss.fff}");
		Solution solution = await workspace.OpenSolutionAsync(solutionPath);
		Console.WriteLine($"Solution opening is completed {DateTime.Now: HH:mm:ss.fff}");
		return solution;
	}

	private static async Task<Solution> UpdateSolution(Document updatedDoc, Solution solution)
	{
		SyntaxNode? node = await updatedDoc.GetSyntaxRootAsync();
		return solution.WithDocumentSyntaxRoot(updatedDoc.Id, node!.NormalizeWhitespace());
	}

	private static async Task<Solution> ProcessProjects(Solution solution)
	{
		foreach (Project project in solution.Projects)
		{
			if (!IsProjectProcessingRequired(project))
			{
				Console.WriteLine($"Ignore project {project.Name}");
				continue;
			}

			Console.WriteLine($"Process project {project.Name} {DateTime.Now: HH:mm:ss.fff}");

			solution = await ProcessDocuments(solution, project);
		}

		return solution;
	}

	private static async Task<Solution> ProcessDocuments(Solution solution, Project project)
	{
		foreach (Document document in project.Documents)
		{
			if (!IsDocumentProcessingRequired(document))
			{
				continue;
			}

			Document updatedDoc = await UpdateDocument(document);
			solution = await UpdateSolution(updatedDoc, solution);
		}

		return solution;
	}

	private static async Task<Document> UpdateDocument(Document document)
	{
		SyntaxNode? syntaxRoot = await document.GetSyntaxRootAsync();
		if (syntaxRoot == null)
			return document;

		Dictionary<MethodDeclarationSyntax, MethodLogInfo> methods = syntaxRoot.DescendantNodes()
			.OfType<MethodDeclarationSyntax>()
			.Select(method => (Method: method, LogInfo: GetMethodLogInfo(method, document)))
			.Where(x => x.LogInfo != null)
			.ToDictionary(x => x.Method, x => x.LogInfo!);

		SyntaxNode newRoot = syntaxRoot.ReplaceNodes(
			methods.Keys,
			(method, _) => ApplyTotalLoggingAspect(method, methods[method])
		);

		return document.WithSyntaxRoot(newRoot);
	}

	private static MethodDeclarationSyntax ApplyTotalLoggingAspect(MethodDeclarationSyntax method, MethodLogInfo logInfo)
	{
		ExpressionStatementSyntax onEnterStatement = CreateOnEnterStatement(method, logInfo);

		if (method.ExpressionBody != null)
		{
			BlockSyntax tryBlock = method.ReturnType.ToString() == "void"
				? SyntaxFactory.Block(SyntaxFactory.ExpressionStatement(method.ExpressionBody.Expression))
				: SyntaxFactory.Block(SyntaxFactory.ReturnStatement(method.ExpressionBody.Expression));

			TryStatementSyntax tryCatchFinallyStatement = CreateTryCatchFinallyStatement(tryBlock, logInfo);

			BlockSyntax newBody = SyntaxFactory.Block(onEnterStatement, tryCatchFinallyStatement);

			return method.WithBody(newBody)
				.WithExpressionBody(null)
				.WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.None));
		}

		if (method.Body != null)
		{
			BlockSyntax tryBlock = SyntaxFactory.Block(method.Body.Statements);

			TryStatementSyntax tryCatchFinallyStatement = CreateTryCatchFinallyStatement(tryBlock, logInfo);

			BlockSyntax newBody = SyntaxFactory.Block(onEnterStatement, tryCatchFinallyStatement);

			return method.WithBody(newBody);
		}

		return method;
	}

	private static ExpressionStatementSyntax CreateOnEnterStatement(MethodDeclarationSyntax method, MethodLogInfo logInfo)
	{
		SeparatedSyntaxList<ParameterSyntax> parameterList = method.ParameterList.Parameters;

		IEnumerable<ParameterSyntax> filteredParameters = parameterList.Where(param =>
				!param.Modifiers.Any(SyntaxKind.OutKeyword) &&
				!param.AttributeLists.Any(attrList =>
					attrList.Attributes.Any(attr => attr.Name.ToString() == "LogIgnoreParameter")))
			.Take(20);

		SeparatedSyntaxList<ArgumentSyntax> arguments = SyntaxFactory.SeparatedList(
			filteredParameters.Select(param =>
				SyntaxFactory.Argument(SyntaxFactory.IdentifierName(param.Identifier.Text))));

		ArgumentSyntax logLevelArg = SyntaxFactory.Argument(
			SyntaxFactory.ParseExpression($"RoslynAspector.TotalLoggingData.LogWrapperLevel.{logInfo.OnEnterLogLevel}"));

		arguments = arguments.Add(logLevelArg);

		InvocationExpressionSyntax onEnterInvocation = SyntaxFactory.InvocationExpression(
				SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
					SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
						SyntaxFactory.IdentifierName("RoslynAspector.TotalLoggingData"),
						SyntaxFactory.IdentifierName("LogWrapper")),
					SyntaxFactory.IdentifierName("OnEnter")))
			.WithArgumentList(SyntaxFactory.ArgumentList(arguments));

		ExpressionStatementSyntax onEnterStatement = SyntaxFactory.ExpressionStatement(onEnterInvocation);
		return onEnterStatement;
	}

	private static TryStatementSyntax CreateTryCatchFinallyStatement(BlockSyntax tryBlock, MethodLogInfo logInfo)
	{
		CatchClauseSyntax catchBlock = CreateCatchBlock(logInfo);

		BlockSyntax finallyBlock = CreateFinallyBlock(logInfo);

		TryStatementSyntax tryCatchFinallyStatement = SyntaxFactory.TryStatement(
			tryBlock,
			SyntaxFactory.List(new[] { catchBlock }),
			SyntaxFactory.FinallyClause(finallyBlock));

		return tryCatchFinallyStatement;
	}

	private static CatchClauseSyntax CreateCatchBlock(MethodLogInfo logInfo)
	{
		CatchDeclarationSyntax catchDeclaration = SyntaxFactory.CatchDeclaration(
			SyntaxFactory.ParseTypeName("System.Exception").WithTrailingTrivia(SyntaxFactory.Space),
			SyntaxFactory.Identifier("aopex"));

		BlockSyntax catchBody = SyntaxFactory.Block(
			SyntaxFactory.ExpressionStatement(
				SyntaxFactory.InvocationExpression(
						SyntaxFactory.MemberAccessExpression(
							SyntaxKind.SimpleMemberAccessExpression,
							SyntaxFactory.MemberAccessExpression(
								SyntaxKind.SimpleMemberAccessExpression,
								SyntaxFactory.IdentifierName("RoslynAspector.TotalLoggingData"),
								SyntaxFactory.IdentifierName("LogWrapper")),
							SyntaxFactory.IdentifierName("OnError")))
					.WithArgumentList(
						SyntaxFactory.ArgumentList(
							SyntaxFactory.SeparatedList(new[]
							{
								SyntaxFactory.Argument(SyntaxFactory.IdentifierName("aopex")),
								SyntaxFactory.Argument(
									SyntaxFactory.ParseExpression($"RoslynAspector.TotalLoggingData.LogWrapperLevel.{logInfo.OnErrorLogLevel}")),
								SyntaxFactory.Argument(
									SyntaxFactory.ParseExpression($"{logInfo.IsExceptionLoggingEnabled}".ToLower())),
							})
						)
					)
			),
			SyntaxFactory.ParseStatement("throw;")
		);

		CatchClauseSyntax catchBlock = SyntaxFactory.CatchClause()
			.WithDeclaration(catchDeclaration)
			.WithBlock(catchBody);
		return catchBlock;
	}

	private static BlockSyntax CreateFinallyBlock(MethodLogInfo logInfo)
	{
		BlockSyntax finallyBlock = SyntaxFactory.Block(
			SyntaxFactory.ExpressionStatement(
				SyntaxFactory.InvocationExpression(
						SyntaxFactory.MemberAccessExpression(
							SyntaxKind.SimpleMemberAccessExpression,
							SyntaxFactory.MemberAccessExpression(
								SyntaxKind.SimpleMemberAccessExpression,
								SyntaxFactory.IdentifierName("RoslynAspector.TotalLoggingData"),
								SyntaxFactory.IdentifierName("LogWrapper")),
							SyntaxFactory.IdentifierName("OnExit")))
					.WithArgumentList(
						SyntaxFactory.ArgumentList(
							SyntaxFactory.SingletonSeparatedList(
								SyntaxFactory.Argument(
									SyntaxFactory.ParseExpression($"RoslynAspector.TotalLoggingData.LogWrapperLevel.{logInfo.OnExitLogLevel}")
								)
							)
						)
					)
			)
		);
		return finallyBlock;
	}

	private static bool IsProjectProcessingRequired(Project project)
	{
		if (project.Documents.FirstOrDefault(d => d.Name.EndsWith("TotalLoggingAspectApply.cs")) != null)
		{
			return true;
		}

		return false;
	}

	private static bool IsDocumentProcessingRequired(Document document)
	{
		if (document.FilePath?.Contains(@"\obj\") != false)
		{
			return false;
		}

		if (document.FilePath?.EndsWith("TotalLoggingAspectApply.cs") != false)
		{
			return false;
		}

		return true;
	}

	private static MethodLogInfo? GetMethodLogInfo(MethodDeclarationSyntax method, Document document)
	{
		if (method.Identifier.Text == "ToString")
			return null;

		SyntaxTokenList modifiers = method.Modifiers;
		bool isPublic = modifiers.Any(m => m.IsKind(SyntaxKind.PublicKeyword));
		bool isInstance = !modifiers.Any(m => m.IsKind(SyntaxKind.StaticKeyword));
		
		LogWrapperLevel onEnterLogLevel = isPublic && isInstance ? LogWrapperLevel.Warning : LogWrapperLevel.Information;
		bool isExceptionLoggingEnabled = isPublic && isInstance;
		
		return new MethodLogInfo
		{
			IsExceptionLoggingEnabled = isExceptionLoggingEnabled,
			OnEnterLogLevel = onEnterLogLevel,
			OnExitLogLevel = LogWrapperLevel.Information,
			OnErrorLogLevel = LogWrapperLevel.Error
		};
	}
}