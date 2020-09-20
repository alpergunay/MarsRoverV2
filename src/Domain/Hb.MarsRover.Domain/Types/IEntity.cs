﻿using System;

namespace Hb.MarsRover.Domain.Types
{
    public interface IEntity<TKey>
    {
        /// <summary>
        /// create date
        /// </summary>
        DateTime CreatedOn { get; }

        /// <summary>
        /// id in string format
        /// </summary>
        TKey Id { get; set; }

        /// <summary>
        /// modify date
        /// </summary>
        DateTime ModifiedOn { get; }
    }
}