using System.Collections.Generic;

public interface IProbabilityPattern 
{
    bool IsHit(int chance);
    IProbabilityPattern Restore();
}