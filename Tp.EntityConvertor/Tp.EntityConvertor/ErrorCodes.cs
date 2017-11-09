namespace Tp.EntityConvertor
{
    /// <summary>
    /// Error codes.
    /// </summary>
    internal static class ErrorCodes
    {
        /// <summary>
        /// Success code.
        /// </summary>
        public static int Ok => 0;

        /// <summary>
        /// Incorrect command line args.
        /// </summary>
        /// <remarks>
        /// The same as EINVAL (http://www.thegeekstuff.com/2010/10/linux-error-codes).
        /// </remarks>
        public static int IncorrectArgs => 22;

        /// <summary>
        /// Error occured during conversion.
        /// Used negative code here to ensure there is no intersection with system error codes.
        /// </summary>
        public static int ConvertError => -2;
    }
}