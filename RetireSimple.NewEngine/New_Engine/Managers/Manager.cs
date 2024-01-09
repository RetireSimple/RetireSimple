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

		protected List<Financial> items;


		public Manager() 
		{
			this.items = new List<Financial>();
		}

		public Projection Calculate(int years) 
		{

			Projection projection = new Projection(new List<double>(), 0);

			foreach (Financial f in this.items) {
				projection = projection.Add(f.Calculate(years));
			}

			return projection;

		}

		public abstract Boolean Add(Financial f); 


		public Financial? Read(int id) 
		{
			if(id < this.items.Count) {

				return this.DoRead(id);
			} else {
				return null;
			}
		}
		public abstract Financial DoRead(int id);

		public Boolean Update(Financial f, int id) 
		{

			if(id < this.items.Count) 
			{
				return this.DoUpdate(f, id);
			}
			else 
			{

				return false;

			}

		}

		public abstract Boolean DoUpdate(Financial f, int id);

		public Boolean Delete(int id) 
		{

			if (id < this.items.Count) 
			{
				return this.DoDelete(id);
			}
			else 
			{
				return false;
			}
			

		}

		public abstract Boolean DoDelete(int id);

	}
}
