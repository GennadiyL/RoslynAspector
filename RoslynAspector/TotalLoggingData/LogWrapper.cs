using System.Runtime.CompilerServices;

namespace RoslynAspector.TotalLoggingData
{
	public static class LogWrapper
	{
#pragma warning disable CS8618
		private static ILogAdapter _adapter;
#pragma warning restore CS8618

		public static void Configure(ILogAdapter adapter)
		{
			_adapter = adapter;
		}
		public static void OnEnter(
			LogWrapperLevel level,
			[CallerFilePath] string file = "",
			[CallerMemberName] string member = "",
			[CallerLineNumber] int line = 0)
		{
			if (_adapter.Level <= level)
				Write($"OnEnter", level, file, member, line);
		}

		public static void OnEnter<T1>(
			T1 t1,
			LogWrapperLevel level,
			[CallerFilePath] string file = "",
			[CallerMemberName] string member = "",
			[CallerLineNumber] int line = 0)
		{
			if (_adapter.Level <= level)
				Write($"OnEnter {t1}", level, file, member, line);
		}

		public static void OnEnter<T1, T2>(
			T1 t1,
			T2 t2,
			LogWrapperLevel level,
			[CallerFilePath] string file = "",
			[CallerMemberName] string member = "",
			[CallerLineNumber] int line = 0)
		{
			if (_adapter.Level <= level)
				Write($"OnEnter {t1} {t2}", level, file, member, line);
		}

		public static void OnEnter<T1, T2, T3>(
			T1 t1,
			T2 t2,
			T3 t3,
			LogWrapperLevel level,
			[CallerFilePath] string file = "",
			[CallerMemberName] string member = "",
			[CallerLineNumber] int line = 0)
		{
			if (_adapter.Level <= level)
				Write($"OnEnter {t1} {t2} {t3}", level, file, member, line);
		}

		public static void OnEnter<T1, T2, T3, T4>(
			T1 t1,
			T2 t2,
			T3 t3,
			T4 t4,
			LogWrapperLevel level,
			[CallerFilePath] string file = "",
			[CallerMemberName] string member = "",
			[CallerLineNumber] int line = 0)
		{
			if (_adapter.Level <= level)
				Write($"OnEnter {t1} {t2} {t3} {t4}", level, file, member, line);
		}

		public static void OnEnter<T1, T2, T3, T4, T5>(
			T1 t1,
			T2 t2,
			T3 t3,
			T4 t4,
			T5 t5,
			LogWrapperLevel level,
			[CallerFilePath] string file = "",
			[CallerMemberName] string member = "",
			[CallerLineNumber] int line = 0)
		{
			if (_adapter.Level <= level)
				Write($"OnEnter {t1} {t2} {t3} {t4} {t5}", level, file, member, line);
		}

		public static void OnEnter<T1, T2, T3, T4, T5, T6>(
			T1 t1,
			T2 t2,
			T3 t3,
			T4 t4,
			T5 t5,
			T6 t6,
			LogWrapperLevel level,
			[CallerFilePath] string file = "",
			[CallerMemberName] string member = "",
			[CallerLineNumber] int line = 0)
		{
			if (_adapter.Level <= level)
				Write($"OnEnter {t1} {t2} {t3} {t4} {t5} {t6}", level, file, member, line);
		}

		public static void OnEnter<T1, T2, T3, T4, T5, T6, T7>(
			T1 t1,
			T2 t2,
			T3 t3,
			T4 t4,
			T5 t5,
			T6 t6,
			T7 t7,
			LogWrapperLevel level,
			[CallerFilePath] string file = "",
			[CallerMemberName] string member = "",
			[CallerLineNumber] int line = 0)
		{
			if (_adapter.Level <= level)
				Write($"OnEnter {t1} {t2} {t3} {t4} {t5} {t6} {t7}", level, file, member, line);
		}

		public static void OnEnter<T1, T2, T3, T4, T5, T6, T7, T8>(
			T1 t1,
			T2 t2,
			T3 t3,
			T4 t4,
			T5 t5,
			T6 t6,
			T7 t7,
			T8 t8,
			LogWrapperLevel level,
			[CallerFilePath] string file = "",
			[CallerMemberName] string member = "",
			[CallerLineNumber] int line = 0)
		{
			if (_adapter.Level <= level)
				Write($"OnEnter {t1} {t2} {t3} {t4} {t5} {t6} {t7} {t8}", level, file, member, line);
		}

		public static void OnEnter<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
			T1 t1,
			T2 t2,
			T3 t3,
			T4 t4,
			T5 t5,
			T6 t6,
			T7 t7,
			T8 t8,
			T9 t9,
			LogWrapperLevel level,
			[CallerFilePath] string file = "",
			[CallerMemberName] string member = "",
			[CallerLineNumber] int line = 0)
		{
			if (_adapter.Level <= level)
				Write($"OnEnter {t1} {t2} {t3} {t4} {t5} {t6} {t7} {t8} {t9}", level, file, member, line);
		}

		public static void OnEnter<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
			T1 t1,
			T2 t2,
			T3 t3,
			T4 t4,
			T5 t5,
			T6 t6,
			T7 t7,
			T8 t8,
			T9 t9,
			T10 t10,
			LogWrapperLevel level,
			[CallerFilePath] string file = "",
			[CallerMemberName] string member = "",
			[CallerLineNumber] int line = 0)
		{
			if (_adapter.Level <= level)
				Write($"OnEnter {t1} {t2} {t3} {t4} {t5} {t6} {t7} {t8} {t9} {t10}", level, file, member, line);
		}

		public static void OnEnter<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
			T1 t1,
			T2 t2,
			T3 t3,
			T4 t4,
			T5 t5,
			T6 t6,
			T7 t7,
			T8 t8,
			T9 t9,
			T10 t10,
			T11 t11,
			LogWrapperLevel level,
			[CallerFilePath] string file = "",
			[CallerMemberName] string member = "",
			[CallerLineNumber] int line = 0)
		{
			if (_adapter.Level <= level)
				Write(
				$"OnEnter {t1} {t2} {t3} {t4} {t5} {t6} {t7} {t8} {t9} {t10} {
					t11}", level, file, member, line);
		}

		public static void OnEnter<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
			T1 t1,
			T2 t2,
			T3 t3,
			T4 t4,
			T5 t5,
			T6 t6,
			T7 t7,
			T8 t8,
			T9 t9,
			T10 t10,
			T11 t11,
			T12 t12,
			LogWrapperLevel level,
			[CallerFilePath] string file = "",
			[CallerMemberName] string member = "",
			[CallerLineNumber] int line = 0)
		{
			if (_adapter.Level <= level)
				Write(
					$"OnEnter {t1} {t2} {t3} {t4} {t5} {t6} {t7} {t8} {t9} {t10} {
						t11} {t12}", level, file, member, line);
		}

		public static void OnEnter<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
			T1 t1,
			T2 t2,
			T3 t3,
			T4 t4,
			T5 t5,
			T6 t6,
			T7 t7,
			T8 t8,
			T9 t9,
			T10 t10,
			T11 t11,
			T12 t12,
			T13 t13,
			LogWrapperLevel level,
			[CallerFilePath] string file = "",
			[CallerMemberName] string member = "",
			[CallerLineNumber] int line = 0)
		{
			if (_adapter.Level <= level)
				Write(
					$"OnEnter {t1} {t2} {t3} {t4} {t5} {t6} {t7} {t8} {t9} {t10} {
						t11} {t12} {t13}", level, file, member, line);
		}

		public static void OnEnter<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
			T1 t1,
			T2 t2,
			T3 t3,
			T4 t4,
			T5 t5,
			T6 t6,
			T7 t7,
			T8 t8,
			T9 t9,
			T10 t10,
			T11 t11,
			T12 t12,
			T13 t13,
			T14 t14,
			LogWrapperLevel level,
			[CallerFilePath] string file = "",
			[CallerMemberName] string member = "",
			[CallerLineNumber] int line = 0)
		{
			if (_adapter.Level <= level)
				Write(
					$"OnEnter {t1} {t2} {t3} {t4} {t5} {t6} {t7} {t8} {t9} {t10} {
						t11} {t12} {t13} {t14}", level, file, member, line);
		}

		public static void OnEnter<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
			T1 t1,
			T2 t2,
			T3 t3,
			T4 t4,
			T5 t5,
			T6 t6,
			T7 t7,
			T8 t8,
			T9 t9,
			T10 t10,
			T11 t11,
			T12 t12,
			T13 t13,
			T14 t14,
			T15 t15,
			LogWrapperLevel level,
			[CallerFilePath] string file = "",
			[CallerMemberName] string member = "",
			[CallerLineNumber] int line = 0)
		{
			if (_adapter.Level <= level)
				Write($"OnEnter {t1} {t2} {t3} {t4} {t5} {t6} {t7} {t8} {t9} {t10} {
					t11} {t12} {t13} {t14} {t15}", level, file, member, line);
		}

		public static void OnEnter<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
			T1 t1,
			T2 t2,
			T3 t3,
			T4 t4,
			T5 t5,
			T6 t6,
			T7 t7,
			T8 t8,
			T9 t9,
			T10 t10,
			T11 t11,
			T12 t12,
			T13 t13,
			T14 t14,
			T15 t15,
			T16 t16,
			LogWrapperLevel level,
			[CallerFilePath] string file = "",
			[CallerMemberName] string member = "",
			[CallerLineNumber] int line = 0)
		{
			if (_adapter.Level <= level)
				Write($"OnEnter {t1} {t2} {t3} {t4} {t5} {t6} {t7} {t8} {t9} {t10} {
					t11} {t12} {t13} {t14} {t15} {t16}", level, file, member, line);
		}

		public static void OnEnter<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(
			T1 t1,
			T2 t2,
			T3 t3,
			T4 t4,
			T5 t5,
			T6 t6,
			T7 t7,
			T8 t8,
			T9 t9,
			T10 t10,
			T11 t11,
			T12 t12,
			T13 t13,
			T14 t14,
			T15 t15,
			T16 t16,
			T17 t17,
			LogWrapperLevel level,
			[CallerFilePath] string file = "",
			[CallerMemberName] string member = "",
			[CallerLineNumber] int line = 0)
		{
			if (_adapter.Level <= level)
				Write($"OnEnter {t1} {t2} {t3} {t4} {t5} {t6} {t7} {t8} {t9} {t10} {
					t11} {t12} {t13} {t14} {t15} {t16} {t17}", level, file, member, line);
		}

		public static void OnEnter<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(
			T1 t1,
			T2 t2,
			T3 t3,
			T4 t4,
			T5 t5,
			T6 t6,
			T7 t7,
			T8 t8,
			T9 t9,
			T10 t10,
			T11 t11,
			T12 t12,
			T13 t13,
			T14 t14,
			T15 t15,
			T16 t16,
			T17 t17,
			T18 t18,
			LogWrapperLevel level,
			[CallerFilePath] string file = "",
			[CallerMemberName] string member = "",
			[CallerLineNumber] int line = 0)
		{
			if (_adapter.Level <= level)
				Write($"OnEnter {t1} {t2} {t3} {t4} {t5} {t6} {t7} {t8} {t9} {t10} {
					t11} {t12} {t13} {t14} {t15} {t16} {t17} {t18}", level, file, member, line);
		}

		public static void OnEnter<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(
			T1 t1,
			T2 t2,
			T3 t3,
			T4 t4,
			T5 t5,
			T6 t6,
			T7 t7,
			T8 t8,
			T9 t9,
			T10 t10,
			T11 t11,
			T12 t12,
			T13 t13,
			T14 t14,
			T15 t15,
			T16 t16,
			T17 t17,
			T18 t18,
			T19 t19,
			LogWrapperLevel level,
			[CallerFilePath] string file = "",
			[CallerMemberName] string member = "",
			[CallerLineNumber] int line = 0)
		{
			if (_adapter.Level <= level)
				Write($"OnEnter {t1} {t2} {t3} {t4} {t5} {t6} {t7} {t8} {t9} {t10} {
					t11} {t12} {t13} {t14} {t15} {t16} {t17} {t18} {t19}", level, file, member, line);
		}

		public static void OnEnter<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20>(
			T1 t1,
			T2 t2,
			T3 t3,
			T4 t4,
			T5 t5,
			T6 t6,
			T7 t7,
			T8 t8,
			T9 t9,
			T10 t10,
			T11 t11,
			T12 t12,
			T13 t13,
			T14 t14,
			T15 t15,
			T16 t16,
			T17 t17,
			T18 t18,
			T19 t19,
			T20 t20,
			LogWrapperLevel level,
			[CallerFilePath] string file = "",
			[CallerMemberName] string member = "",
			[CallerLineNumber] int line = 0)
		{
			if (_adapter.Level <= level)
				Write($"OnEnter {t1} {t2} {t3} {t4} {t5} {t6} {t7} {t8} {t9} {t10} {
					t11} {t12} {t13} {t14} {t15} {t16} {t17} {t18} {t19} {t20}", level, file, member, line);
		}

		public static void OnError(
			Exception ex,
			LogWrapperLevel level,
			bool isExceptionLoggingEnabled,
			[CallerFilePath] string file = "",
			[CallerMemberName] string member = "",
			[CallerLineNumber] int line = 0)
		{
			if (_adapter.Level <= level)
				Write("OnError", isExceptionLoggingEnabled ? ex : null, level, file, member, line);
		}

		public static void OnExit(
			LogWrapperLevel level,
			[CallerFilePath] string file = "",
			[CallerMemberName] string member = "",
			[CallerLineNumber] int line = 0)
		{
			if (_adapter.Level <= level)
				Write("OnExit", level, file, member, line);
		}

		private static void Write(string message, LogWrapperLevel level, string file, string member, int line)
		{
			Write(message, null, level, file, member, line);
		}

		private static void Write(string message, Exception? ex, LogWrapperLevel level, string file, string member, int line)
		{
			_adapter.Write(message, ex, level, file, member, line);
		}
	}
}
