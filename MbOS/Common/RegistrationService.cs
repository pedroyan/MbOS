using System;
using System.Collections.Generic;
using System.Text;

namespace MbOS.Common {
	public static class RegistrationService {
		static Dictionary<Type, object> RegistrationDictionary = new Dictionary<Type, object>();

		/// <summary>
		/// Registra uma Implementação para a interface passada
		/// </summary>
		/// <typeparam name="T">Tipo da interface</typeparam>
		/// <param name="Instance">Implementação concreta da interface</param>
		public static void RegisterInstance<T>(T Instance) {
			var type = typeof(T);

			if (!type.IsInterface) {
				throw new ArgumentException("O tipo T precisa ser uma interface");
			}

			if (RegistrationDictionary.ContainsKey(type)) {
				RegistrationDictionary[type] = Instance;
			} else {
				RegistrationDictionary.Add(type, Instance);
			}
		}

		/// <summary>
		/// Busca uma implementação registrada para a interface passada
		/// </summary>
		/// <typeparam name="T">Interface que se deseja obter uma implementação</typeparam>
		/// <returns>Uma implementação para a interface</returns>
		public static T Resolve<T>() {
			var type = typeof(T);
			if (!type.IsInterface) {
				throw new ArgumentException("O tipo T precisa ser uma interface");
			}

			if (!RegistrationDictionary.ContainsKey(type)) {
				throw new ArgumentException("Nenhuma implementação para a interface passada");
			}

			return (T)RegistrationDictionary[type];
		}
	}
}
