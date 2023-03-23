using System;
using System.Threading;

class TrafficLight
{
    private object _lock = new object();
    private int _currentLightIndex = 0;
    private string[] _lights = { "Green", "Yellow", "Red" };

    public void ChangeLight()
    {
        lock (_lock)
        {
            _currentLightIndex = (_currentLightIndex + 1) % 3;
            Console.WriteLine($"Light changed to {_lights[_currentLightIndex]}");
        }
    }

    public string GetCurrentLight()
    {
        return _lights[_currentLightIndex];
    }
}

class Car
{
    private int _id;
    private TrafficLight[] _trafficLights;

    public Car(int id, TrafficLight[] trafficLights)
    {
        _id = id;
        _trafficLights = trafficLights;
    }

    public void Drive()
    {
        while (true)
        {
            int allowedCars = 2;
            foreach (TrafficLight trafficLight in _trafficLights)
            {
                if (trafficLight.GetCurrentLight() != "Green")
                {
                    allowedCars = 0;
                    break;
                }
            }

            if (allowedCars > 0)
            {
                Console.WriteLine($"Car {_id} is driving");
                allowedCars--;
            }

            Thread.Sleep(1000);
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        TrafficLight[] trafficLights = new TrafficLight[4];
        for (int i = 0; i < trafficLights.Length; i++)
        {
            trafficLights[i] = new TrafficLight();
        }

        Car[] cars = new Car[10];
        for (int i = 0; i < cars.Length; i++)
        {
            cars[i] = new Car(i, trafficLights);
            new Thread(cars[i].Drive).Start();
        }

        while (true)
        {
            Thread.Sleep(5000);
            foreach (TrafficLight trafficLight in trafficLights)
            {
                trafficLight.ChangeLight();
            }
        }
    }
}
