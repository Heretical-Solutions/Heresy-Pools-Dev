using Zenject;
using UnityEngine;
using System.Collections;
using HereticalSolutions.Messaging;
using HereticalSolutions.Collections;
using HereticalSolutions.Pools.Messages;
using HereticalSolutions.Pools.DAO;

namespace HereticalSolutions.Pools.Behaviours
{
	public enum EResolutionBehaviour
	{
		EXTERNAL_ONLY,
		IMMEDIATELY,
		RESOLVE_AFTER_TICKS,
		RESOLVE_AFTER_TICKS_IN_RANGE
	}
}