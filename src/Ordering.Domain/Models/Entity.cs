namespace Ordering.Domain.Models;

/// <summary>
/// Entity
/// </summary>
public abstract class Entity
{
    /// <summary>
    /// The identifier
    /// </summary>
    int _id;

    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>
    /// The identifier.
    /// </value>
    public virtual int Id
    {
        get
        {
            return _id;
        }
        protected set
        {
            _id = value;
        }
    }
}
