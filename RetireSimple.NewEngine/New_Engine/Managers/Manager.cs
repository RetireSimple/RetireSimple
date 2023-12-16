using RetireSimple.Engine.New_Engine;
using RetireSimple.NewEngine.New_Engine.Financials;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RetireSimple.NewEngine.New_Engine.Managers {
	abstract public class Manager {

		private List<Financial> items;


		public Manager() 
		{
			this.items = new List<Financial>();
		}

		public Projection Calculate(int years) 
		{

			Projection projection = new Projection(new List<double>());

			foreach (Financial f in this.items) {
				projection = projection.Add(f.Calculate(years));
			}

			return projection;

		}

		public Boolean Add(Financial f) 
		{
			this.items.Add(f);
			return true;

		}

		public Financial Read(int id) 
		{
			return this.items[id];
		}

		public Boolean Update(Financial f, int id) 
		{

			if(id < this.items.Count) 
			{
				this.items[id] = f;

				return true;
			}
			else 
			{

				return false;

			}



		}

		public Boolean Delete(int id) 
		{

			if (id < this.items.Count) 
			{
				this.items.RemoveAt(id);
				return true;
			}
			else 
			{
				return false;
			}
			


		}

	}
}
