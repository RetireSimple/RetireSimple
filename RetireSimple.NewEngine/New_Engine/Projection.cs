using System.Collections    ;
namespace RetireSimple.Engine.New_Engine
{
    public class Projection
    {

        public List<double> yearly_projections;

        public Projection(List<double> list)
        {
            this.yearly_projections = list;
        }

        public Projection Add(Projection other_proj)
        {
            List<double> list = new List<double>();

            int count;

            if(this.yearly_projections.Count > other_proj.yearly_projections.Count)
            {   
                count = this.yearly_projections.Count;

            }
            else{
                count = other_proj.yearly_projections.Count;

            }
            for(int i = 0; i < count; i++)
                {
                    if (i >= other_proj.yearly_projections.Count)
                    {
                        list.Add(this.yearly_projections[i]);
                    }
                    else if (i >= this.yearly_projections.Count){
                        list.Add(other_proj.yearly_projections[i]);
                    }
                    else 
                    {
                        list.Add(this.yearly_projections[i] + other_proj.yearly_projections[i]);
                    }
                }

                return new Projection(list);
        }

    }
    
}