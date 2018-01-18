﻿namespace Tangle.Net.Source.Repository
{
  using System;

  /// <summary>
  /// The iri api exception.
  /// </summary>
  public class IriApiException : ApplicationException
  {
    #region Constructors and Destructors

    /// <summary>
    /// Initializes a new instance of the <see cref="IriApiException"/> class.
    /// </summary>
    /// <param name="message">
    /// The message.
    /// </param>
    public IriApiException(string message)
      : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="IriApiException"/> class.
    /// </summary>
    /// <param name="message">
    /// The message.
    /// </param>
    /// <param name="innerException">
    /// The inner exception.
    /// </param>
    public IriApiException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    #endregion
  }
}