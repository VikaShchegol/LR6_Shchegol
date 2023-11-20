using System;

class Viewer
{
    private int viewerNumber;

    public int ViewerNumber
    {
        get { return viewerNumber; }
    }

    public Viewer(int number)
    {
        viewerNumber = number;
    }
}

class Cinema
{
    private int totalSeats;
    private int occupiedSeats;
    public event Action NotEnoughPlaces;
    public event Action NotPlaces;
    public Cinema(int seats, string filmName)
    {
        totalSeats = seats;
        occupiedSeats = 0;
        Hardware hardware = new Hardware(filmName);
        NotPlaces += hardware.FilmOn;
    }
    public void PushViewer(Viewer viewer)
    {
        if (occupiedSeats < totalSeats)
        {
            occupiedSeats++;
            Console.WriteLine($"Глядач {viewer.ViewerNumber} зайняв своє мiсце.");

            if (occupiedSeats == totalSeats)
            {
                NotPlaces?.Invoke();
            }
        }
        else
        {
            Console.WriteLine($"Глядачiв бiльше, нiж мiсць.");
            NotEnoughPlaces?.Invoke(); 
        }
    }
}

class Security
{
    public void CloseZal()
    {
        Console.WriteLine("Дежурний закрив зал.");
        if (NotPlaces != null)
        {
            NotPlaces();
        }
    }

    public event Action NotPlaces;
}

class Light
{
    public void Turn()
    {
        Console.WriteLine("Вимикаємо свiтло!");
        if (SwitchOff != null)
        {
            SwitchOff();
        }
    }

    public event Action SwitchOff;
}

class Hardware
{
    private string filmName;

    public Hardware(string name)
    {
        filmName = name;
    }
    public void FilmOn()
    {
        Console.WriteLine($"Починається фiльм {filmName}");
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("Введiть кiлькiсть мiсць у залi:");
        int seats = int.Parse(Console.ReadLine());

        Console.WriteLine("Введiть назву фiльму:");
        string filmName = Console.ReadLine();

        Cinema cinema = new Cinema(seats, filmName);
        Security security = new Security();
        Light light = new Light();

        cinema.NotPlaces += security.CloseZal;
        security.NotPlaces += light.Turn;
        cinema.NotEnoughPlaces += () => Console.WriteLine("Не вистачає мiсць для всiх глядачiв.");

        Console.WriteLine("Введiть кiлькiсть глядачiв:");
        int numberOfViewers = int.Parse(Console.ReadLine());

        for (int i = 1; i <= numberOfViewers; i++)
        {
            Viewer viewer = new Viewer(i);
            cinema.PushViewer(viewer);
        }

        Console.ReadLine(); 
    }
}
