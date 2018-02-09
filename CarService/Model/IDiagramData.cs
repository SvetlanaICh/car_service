using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarService.Model
{
    public interface IDiagramData
    {
		int Year { get; set; }
		List<int> Values { get; set; }
		List<KeyValuePair<string, int>> DataForDiagramCarBrand { get; }
        List<KeyValuePair<string, int>> DataForDiagramMonth { get; }
        List<KeyValuePair<string, int>> DataForDiagramPrice { get; }
    }
}
