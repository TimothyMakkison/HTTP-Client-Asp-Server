namespace HTTP_Client_Asp_Server.Models.CommandModel
{
    /// <summary>
    /// Enum containing parsing modes for console inputs.
    /// </summary>
    public enum ParseMode
    {
        /// <summary>
        /// Do not parse input.
        /// </summary>
        None,

        /// <summary>
        /// Remove CommandKey from input string.
        /// </summary>
        Parse,

        /// <summary>
        /// Remove CommandKey and trim spaces from input string.
        /// </summary>
        ParseAndTrim
    }
}