using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.MSBuild;
using Project = Microsoft.CodeAnalysis.Project;

namespace RoslynAspector.TotalLogging;

internal class Program
{
	private const string _solutionPath = "..\\..\\..\\..\\..\\RoslynLoggingDemoApp\\RoslynLoggingDemoApp.sln";

	public static async Task Main(string[] args)
	{
		MSBuildLocator.RegisterDefaults();
		MSBuildWorkspace workspace = MSBuildWorkspace.Create();

		Console.WriteLine($"Open the Solution {DateTime.Now: HH:mm:ss.fff}");
		Solution solution = await workspace.OpenSolutionAsync(_solutionPath);
		Console.WriteLine($"Solution opening is completed {DateTime.Now: HH:mm:ss.fff}");

		foreach (Project project in solution.Projects)
		{
			if (!IsProjectProcessingRequired(project))
			{
				Console.WriteLine($"Ignore {project.Name}");
				continue;
			}

			Console.WriteLine($"Apply {project.Name} {DateTime.Now: HH:mm:ss.fff}");

			foreach (Document document in project.Documents)
			{
				if (!IsDocumentProcessingRequired(document))
				{
					continue;
				}

				Document updatedDoc = await ProcessDocument(document);
				solution = await UpdateSolution(updatedDoc, solution);
			}
		}

		Console.WriteLine($"Update the Solution {DateTime.Now: HH:mm:ss.fff}");
		workspace.TryApplyChanges(solution);
		Console.WriteLine($"Solution updating is completed {DateTime.Now: HH:mm:ss.fff}");
	}

	private static async Task<Document> ProcessDocument(Document document)
	{
		SyntaxNode? syntaxRoot = await document.GetSyntaxRootAsync();
		if (syntaxRoot == null)
		{
			return document;
		}

		IEnumerable<MethodDeclarationSyntax> methodDeclarations = syntaxRoot.DescendantNodes()
			.OfType<MethodDeclarationSyntax>()
			.Where(IsMethodProcessingRequired);

		SyntaxNode newRoot = syntaxRoot.ReplaceNodes(methodDeclarations,
			(original, _) => AddTryCatchFinallyAspect(original));

		return document.WithSyntaxRoot(newRoot);
	}

	private static async Task<Solution> UpdateSolution(Document updatedDoc, Solution solution)
	{
		SyntaxNode? node = await updatedDoc.GetSyntaxRootAsync();
		return solution.WithDocumentSyntaxRoot(updatedDoc.Id, node!.NormalizeWhitespace());
	}

	private static MethodDeclarationSyntax AddTryCatchFinallyAspect(MethodDeclarationSyntax method)
	{
		SeparatedSyntaxList<ParameterSyntax> parameterList = method.ParameterList.Parameters;

		ExpressionStatementSyntax onEnterStatement = CreateOnEnterAdvice(parameterList);

		string returnType = method.ReturnType.ToString();

		if (method.ExpressionBody != null)
		{
			ExpressionSyntax expression = method.ExpressionBody.Expression;

			BlockSyntax tryBlock;

			if (returnType == "void")
			{
				tryBlock = SyntaxFactory.Block(SyntaxFactory.ExpressionStatement(expression));
			}
			else
			{
				tryBlock = SyntaxFactory.Block(SyntaxFactory.ReturnStatement(expression));
			}

			TryStatementSyntax tryCatch = CreateTryCatchFinallyStatement(tryBlock);

			BlockSyntax newBody = SyntaxFactory.Block(onEnterStatement, tryCatch);

			return method.WithBody(newBody)
				.WithExpressionBody(null)
				.WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.None));
		}

		if (method.Body != null)
		{
			BlockSyntax tryBlock;

			if (returnType == "void")
			{
				tryBlock = SyntaxFactory.Block(method.Body.Statements);
			}
			else
			{
				tryBlock = SyntaxFactory.Block(method.Body.Statements);
			}

			TryStatementSyntax tryCatch = CreateTryCatchFinallyStatement(tryBlock);

			BlockSyntax newBody = SyntaxFactory.Block(onEnterStatement, tryCatch);

			return method.WithBody(newBody);
		}

		return method;
	}

	private static TryStatementSyntax CreateTryCatchFinallyStatement(BlockSyntax tryBlock)
	{
		CatchDeclarationSyntax catchDecl = SyntaxFactory.CatchDeclaration(
			SyntaxFactory.ParseTypeName("System.Exception").WithTrailingTrivia(SyntaxFactory.Space),
			SyntaxFactory.Identifier("aopex"));

		BlockSyntax catchBlock = SyntaxFactory.Block(
			SyntaxFactory.ParseStatement("RoslynGeneratorData.Logger.Instance.OnError(aopex);"),
			SyntaxFactory.ParseStatement("throw;"));

		CatchClauseSyntax catchClause = SyntaxFactory.CatchClause()
			.WithDeclaration(catchDecl)
			.WithBlock(catchBlock);

		BlockSyntax finallyBlock = SyntaxFactory.Block(
			SyntaxFactory.ParseStatement("RoslynGeneratorData.Logger.Instance.OnExit();"));

		TryStatementSyntax tryCatchFinally = SyntaxFactory.TryStatement(
			tryBlock,
			SyntaxFactory.List(new[] { catchClause }),
			SyntaxFactory.FinallyClause(finallyBlock));

		return tryCatchFinally;
	}

	private static ExpressionStatementSyntax CreateOnEnterAdvice(SeparatedSyntaxList<ParameterSyntax> parameters)
	{
		IEnumerable<ParameterSyntax> filteredParameters = parameters.Where(param =>
				!param.Modifiers.Any(SyntaxKind.OutKeyword) &&
				!param.AttributeLists.Any(attrList =>
					attrList.Attributes.Any(attr => attr.Name.ToString() == "IgnoreParameter")))
			.Take(20);

		SeparatedSyntaxList<ArgumentSyntax> arguments = SyntaxFactory.SeparatedList(
			filteredParameters.Select(param =>
				SyntaxFactory.Argument(SyntaxFactory.IdentifierName(param.Identifier.Text))));

		InvocationExpressionSyntax onEnterInvocation = SyntaxFactory.InvocationExpression(
				SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
					SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
						SyntaxFactory.IdentifierName("RoslynGeneratorData"),
						SyntaxFactory.IdentifierName("Logger")),
					SyntaxFactory.IdentifierName("Instance.OnEnter")))
			.WithArgumentList(SyntaxFactory.ArgumentList(arguments));

		ExpressionStatementSyntax onEnterStatement = SyntaxFactory.ExpressionStatement(onEnterInvocation);

		return onEnterStatement;
	}

	private static bool IsProjectProcessingRequired(Project project)
	{
		if (project.Documents.FirstOrDefault(d => d.Name.Contains("AspectApply")) != null)
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

		if (document.FilePath?.EndsWith("AspectApply.cs") != false)
		{
			return false;
		}
		return true;
	}

	private static bool IsMethodProcessingRequired(MethodDeclarationSyntax method)
	{
		return true;
	}
}
