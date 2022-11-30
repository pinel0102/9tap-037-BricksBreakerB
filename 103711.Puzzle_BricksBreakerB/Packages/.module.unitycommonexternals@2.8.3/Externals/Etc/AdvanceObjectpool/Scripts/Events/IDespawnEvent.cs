using UnityEngine;

// FIXME: dante (AOP 네임 스페이스 추가) {
namespace AOP {
	public interface IDespawnEvent
	{
		void OnDespawned(GameObject targetGameObject, ObjectPool sender);
	}
}
// FIXME: dante (AOP 네임 스페이스 추가) }
