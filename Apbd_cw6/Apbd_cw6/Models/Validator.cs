namespace Apbd_cw6.Models;

public class Validator
{
    public static bool CheckArgument(string argument)
    {
        List<string> allowedArguments = new List<string>() { "name", "category", "description", "area"};
        
        if (!allowedArguments.Contains(argument.ToLower()))
            return false;

        return true;
    }
}
