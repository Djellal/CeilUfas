using CeilUfas.Data;

namespace CeilUfas
{
    public partial class ceilufasService
    {
        public ceilufasContext dbContext
        {
            get
            {
                return this.context;
            }
        }
    }
}
