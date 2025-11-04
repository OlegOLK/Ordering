namespace Ordering.API.Models.Output
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TData">The type of the data.</typeparam>
    public class Response<TData>
    {
        /// <summary>
        /// Gets or sets the result.
        /// </summary>
        /// <value>
        /// The result.
        /// </value>
        public TData? Result { get; set; } = default;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Response{TData}"/> is success.
        /// </summary>
        /// <value>
        ///   <c>true</c> if success; otherwise, <c>false</c>.
        /// </value>
        public bool Success { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Response{TData}"/> class.
        /// </summary>
        /// <param name="success">if set to <c>true</c> [success].</param>
        protected Response(bool success)
        {
            Success = success;
            Result = default;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Response{TData}"/> class.
        /// </summary>
        /// <param name="result">The result.</param>
        protected Response(TData result) : this(true)
        {
            Result = result;
        }

        /// <summary>
        /// Oks the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static Response<TData> Ok(TData data) => new Response<TData>(data);

        /// <summary>
        /// Oks this instance.
        /// </summary>
        /// <returns></returns>
        public static Response<TData> Ok() => new Response<TData>(true);

        /// <summary>
        /// Fails this instance.
        /// </summary>
        /// <returns></returns>
        public static Response<TData> Fail() => new Response<TData>(false);
    }
}
