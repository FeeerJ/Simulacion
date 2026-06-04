using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulacion.Application.Dtos;

public record AdditiveCongruentialModel(long M,long N0,long N1,int TotalNumeros);

public record MultiplicativeCongruentialModel(long M,long N0,long A,int TotalNumeros);

public record MixedCongruentialModel(long M,long N0, long A, long C, int TotalNumeros);


