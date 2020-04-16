using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kpi.Core.Helper
{
    public class AutoMapperHelper
    {
        /// <summary>
        /// Mapping with same class between object and map
        /// </summary>
        public static TDest Map<TSource, TDest>(TSource source) where TDest : class where TSource : class
        {
            if (source == null) return null;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TSource, TDest>();
            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<TSource, TDest>(source);
        }

        /// <summary>
        /// Mapping with different class between object and map
        /// </summary>
        public static TDest Map<TSourceMap, TDestMap, TSource, TDest>(TSource source) where TDest : class where TSource : class
        {
            if (source == null) return null;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TSourceMap, TDestMap>();
            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<TSource, TDest>(source);
        }

        /// <summary>
        /// Mapping with different class between object and map
        /// </summary>
        public static void MapSameType<TSource>(object source, object dest, Type type)
        {
            if (source == null) return;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TSource, TSource>();
            });

            IMapper mapper = config.CreateMapper();
            mapper.Map(source, dest, type, type);
        }
    }
}
