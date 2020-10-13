using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlsPropertyAnalysis
{
    class Neighborhood
    {
        public Neighborhood(int zipCode, List<NeighborhoodGrade> grades)
        {
            ZipCode = zipCode;
            Grades = grades;
        }

        public int ZipCode { get; set; }
        public List<NeighborhoodGrade> Grades { get; set; }
    }

    class NeighborhoodGrade
    {
        public NeighborhoodGrade(Grades grade, List<string> cities)
        {
            Grade = grade;
            Cities = cities;
        }

        public Grades Grade { get; set; }
        public List<string> Cities { get; set; }
    }

    public enum Grades
    {
        A = 1,
        B = 2,
        C = 3,
        D = 4,
        F = 5
    }

    class Neighborhoods
    {
        public Neighborhood GetZipCodeGrades(int zipCode)
        {
            List<NeighborhoodGrade> grades = new List<NeighborhoodGrade>();
            if (A.Keys.Contains(zipCode))
            {
                List<string> cities = A[zipCode];
                grades.Add(new NeighborhoodGrade(Grades.A,cities));
            }
            if (B.Keys.Contains(zipCode))
            {
                string city = B[zipCode];
                grades.Add(new NeighborhoodGrade(Grades.B, new List<string> { city }));
            }
            if (C.Keys.Contains(zipCode))
            {
                List<string> cities = C[zipCode];
                grades.Add(new NeighborhoodGrade(Grades.C, cities));
            }
            if (D.Keys.Contains(zipCode))
            {
                string city = D[zipCode];
                grades.Add(new NeighborhoodGrade(Grades.D, new List<string> { city }));
            }
            if (F.Keys.Contains(zipCode))
            {
                string city = F[zipCode];
                grades.Add(new NeighborhoodGrade(Grades.F, new List<string> { city }));
            }

            return new Neighborhood(zipCode, grades);
        }

        private readonly Dictionary<int, List<string>> A = new Dictionary<int, List<string>>
        {
            { 44040, new List<string> { "Hunting Valley", "Gates Mills" } },
            { 44022, new List<string> {"Bentleyville", "Orange", "Chagrin Falls" } },
            { 44124, new List<string> {"Pepper Pike"} },            
            { 44236, new List<string> {"Hudson", "Macedonia" } },
            { 44094, new List<string> {"Waite Hill", "Kirtland" } },            
            { 44139, new List<string> {"Solon" } },
            { 44108, new List<string> {"Bratenahl" } },
            { 44122, new List<string> {"Beachwood", "Shaker Heights" } },
            { 44012, new List<string> {"Avon Lake"} },                   
            { 44140, new List<string> {"Bay Village"} },
            { 44011, new List<string> {"Avon "} },
            { 44149, new List<string> {"Strongsville" } },
            { 44136, new List<string> {"Strongsville "} },
            { 44145, new List<string> {"Westlake "} },
            { 44147, new List<string> {"Broadview Heights"} },            
            { 44120, new List<string> {"Shaker Heights"} },
            { 44133, new List<string> {"North Royalton"} },
            { 44118, new List<string> {"University Heights"} },
            { 44070, new List<string> {"North Olmsted"} },
            { 44130, new List<string> {"Middleburg Heights"} },            
            { 44023, new List<string> {"Chagrin Falls" } }
        };

        private readonly Dictionary<int, string> B = new Dictionary<int, string>
        {
            { 44121, "South Euclid"},
            { 44118, "Cleveland Heights"},
            { 44112, "Cleveland Heights"},
            { 44129, "Parma "},
            { 44130, "Parma "},
            { 44134, "Parma "},
            { 44142, "Brook Park"},
            { 44017, "Berea "},
            { 44107, "Lakewood "},
            { 44137, "Garfield Heights"},
            { 44125, "Garfield Heights"},
            { 44144, "Brooklyn" },
            { 44117, "Euclid "},
            { 44123, "Euclid "},
            { 44132, "Euclid "},
            { 44113, "Cleveland" }
        };

        private readonly Dictionary<int, List<string>> C = new Dictionary<int, List<string>>
        {
            { 44053, new List<string>{"Lorain" } },
            { 44035, new List<string>{"Elyria " } },
            { 44036, new List<string>{"Elyria"} },
            { 44111, new List<string>{"Cleveland " } },
            { 44144, new List<string>{"Cleveland" } },
            { 44146, new List<string>{"Bedford "} },
            { 44135, new List<string>{"Cleveland "} },
            { 44128, new List<string>{"Bedford Heights", "Warrensville Heights" } },
            { 44137, new List<string>{"Maple Heights"} },            
            { 44109, new List<string>{"Cleveland"} },
            { 44105, new List<string>{"Newburgh Heights", "Garfield Heights" } },
            { 44122, new List<string>{"Cleveland"} }
        };

        private readonly Dictionary<int, string> D = new Dictionary<int, string>
        {
            { 44052, "Lorain "},
            { 44120, "Cleveland "},
            { 44055, "Lorain "},
            { 44109, "Cleveland "},
            { 44113, "Cleveland "},
            { 44102, "Cleveland "}
        };

        private readonly Dictionary<int, string> F = new Dictionary<int, string>
        {
            { 44105, "Cleveland "},
            { 44112, "East Cleveland"},
            { 44106, "Cleveland "},
            { 44110, "Cleveland "},
            { 44108, "Cleveland "},
            { 44114, "Cleveland "},
            { 44127, "Cleveland "},
            { 44103, "Cleveland "},
            { 44104, "Cleveland "},
            { 44115, "Cleveland "}
        };


    }
}
