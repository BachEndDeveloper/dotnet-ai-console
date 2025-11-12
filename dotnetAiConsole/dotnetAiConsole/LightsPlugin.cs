using System.ComponentModel;
using Microsoft.SemanticKernel;

namespace dotnetAiConsole;

public class LightsPlugin
{
    [KernelFunction, Description("Turn on the lights in a specified location.")]
    public string TurnOn(string location)
    {
        return $"The lights in the {location} have been turned on.";
    }

    [KernelFunction, Description("Turn off the lights in a specified location.")]
    public string TurnOff(string location)
    {
        return $"The lights in the {location} have been turned off.";
    }
}