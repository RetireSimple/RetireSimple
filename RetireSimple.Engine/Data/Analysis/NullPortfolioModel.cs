using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using RetireSimple.Engine.Data.User;

using System.Text.Json;
using System.Text.Json.Serialization;

namespace RetireSimple.Engine.Data.Analysis {
    public class NullPortfolioModel : PortfolioModel {

        public NullPortfolioModel(){
            this.PortfolioModelId = -1;
        }

    }


}