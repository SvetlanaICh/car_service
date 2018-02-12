﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarService.Model.Experiments
{
	public interface IOrderExtendedPredicats
	{
		Predicate<OrderExtended> GetPredicate(string aCondition, string aValue);
	}
}
