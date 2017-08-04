using System;
using UnityEngine;
using System.Collections.Generic;

namespace SDTM
{
	

	public static class EntityUtils
	{

		public static class ENTITIES{
			public static int SUPPLY_PLANE=-25819687;	
		}

		public static void SpawnEntity(string entityName, Vector3 pos){
			using (Dictionary<int, EntityClass>.KeyCollection.Enumerator enumerator3 = EntityClass.list.Keys.GetEnumerator ()) {
				while (enumerator3.MoveNext ()) {
					int current3 = enumerator3.Current;

					if (EntityClass.list [current3].entityClassName.Equals (entityName)){
						Entity entity = EntityFactory.CreateEntity (current3, pos);

						if (entity is EntityVehicle) {
							((EntityVehicle)entity).PopulatePartData ();
						}
						GameManager.Instance.World.SpawnEntityInWorld (entity);	
						return;
					}
				}
			}
		}

		public static void SpawnEntity(int _entityType, Vector3 pos){
			//need to loop entitclas list keys to get the right id
			int num=1;

			using (Dictionary<int, EntityClass>.KeyCollection.Enumerator enumerator3 = EntityClass.list.Keys.GetEnumerator ()) {
				while (enumerator3.MoveNext ()) {
					int current3 = enumerator3.Current;
					//if (EntityClass.list [current3].bAllowUserInstantiate) {
						if (num != _entityType) {
							//if (!EntityClass.list [current3].entityClassName.Equals (_params [1])) {
								num++;
								continue;
							//}
						}

						Entity entity = EntityFactory.CreateEntity (current3, pos);

						if (entity is EntityVehicle) {
							((EntityVehicle)entity).PopulatePartData ();
						}
						GameManager.Instance.World.SpawnEntityInWorld (entity);
						//SingletonMonoBehaviour<SdtdConsole>.Instance.Output (NW.MM (146467) + EntityClass.list [current3].entityClassName);
						return;
					//}
				}
			}
		}
	}
}

