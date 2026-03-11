

using System.Threading.Tasks;
using System.Web.Http;

namespace MVCMarketing.Models
{
    public class BindListController : ApiController
    {
        [Route("api/BindList/DropDown")]
        [HttpPost]
        public async Task<string> DropDown(string action, string refId)
        {
            //string jsonString = ConnectionClass.fillDropDown("sp_" + sp, "SELECTDROPDOWN", refId);// SP , Action            
            string jsonString = await Task.Run(() => ConnectionClass.fillDropDown("sp_" + action, "SELECTDROPDOWN", refId));
            return jsonString;
        }

        [Route("api/BindList/AutoComplete")]
        [HttpPost]
        public async Task<string> AutoComplete(string prefix,string action, string refId)
        {
            //string jsonString = ConnectionClass.fillDropDown("sp_" + sp, "SELECTDROPDOWN", refId);// SP , Action            
            string jsonString = await Task.Run(() => ConnectionClass.fillAutoComplete("sp_AutoComplete" + action, prefix, refId));
            return jsonString;
        }
    }
}