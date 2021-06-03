using System.Collections.Generic;
using System.IO;
using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using TourPlanner.Models;
using System.Linq;
namespace TourPlanner.BusinessLayer
{
    public class TourLogReport : IDocument
    {
        private Tour _tour;
        private List<TourLog> _tourlogs;
        public TourLogReport(Tour t, List<TourLog> logs)
        {
            _tour = t;
            _tourlogs = logs;
        }
        public void Compose(IContainer container)
        {
            container.AlignLeft().Page((page => 
            {
                page.Header().Stack(stack =>
                {
                    stack.Item().Text($"Name: " + _tour.Name);
                    stack.Item().Text($"Description: " + _tour.Description);
                    stack.Item().Text($"Distance: " + _tour.Distance);
                    stack.Item().Text($"Start Point: " + _tour.StartPoint);
                    stack.Item().Text($"End Point: " + _tour.EndPoint);
                    stack.Item().MaxWidth(250).Image(File.ReadAllBytes(_tour.RouteInformation));
                });
                page.Content().Decoration(decoration =>
                {
                    decoration.Header().BorderBottom(1).Padding(5).Row(row =>
                    {
                        row.RelativeColumn().Text("Date");
                        row.RelativeColumn().Text("Report");
                        row.RelativeColumn().Text("Distance");
                        row.RelativeColumn().Text("Time");
                        row.RelativeColumn().Text("Rating");
                        row.RelativeColumn().Text("AvgSpeed");
                        row.RelativeColumn().Text("BurnedJoule");
                        row.RelativeColumn().Text("Difficulty");
                        row.RelativeColumn().Text("Height Delta");
                        row.RelativeColumn().Text("Max Speed");
                    });
                    decoration.Content().Stack(stack =>
                    {
                        foreach (TourLog log in _tourlogs)
                        {
                            stack.Item().BorderBottom(1).BorderColor("CCC").Padding(5).Row(row =>
                            {
                                row.RelativeColumn().Text(log.Date);
                                row.RelativeColumn().Text(log.Report);
                                row.RelativeColumn().Text(log.Distance);
                                row.RelativeColumn().Text(log.Time);
                                row.RelativeColumn().Text(log.Rating);
                                row.RelativeColumn().Text(log.AvgSpeed);
                                row.RelativeColumn().Text(log.BurnedJoule);
                                row.RelativeColumn().Text(log.Difficulty);
                                row.RelativeColumn().Text(log.HeightDelta);
                                row.RelativeColumn().Text(log.MaxSpeed);
                            });
                        }
                        stack.Item().BorderBottom(1).BorderColor("CCC").Padding(5).Row(row =>
                        {
                            row.RelativeColumn().Text("");
                            row.RelativeColumn().Text("");
                            row.RelativeColumn().Text(_tourlogs.Sum(tl => tl.Distance));
                            row.RelativeColumn().Text("Minutes: " + _tourlogs.Sum(tl => tl.Time.TotalMinutes));
                            row.RelativeColumn().Text((double)_tourlogs.Sum(tl => tl.Rating) / _tourlogs.Count);
                            row.RelativeColumn().Text(_tourlogs.Sum(tl => tl.AvgSpeed) / _tourlogs.Count);
                            row.RelativeColumn().Text((double)_tourlogs.Sum(tl => tl.BurnedJoule) / _tourlogs.Count);
                            row.RelativeColumn().Text((double)_tourlogs.Sum(tl => tl.Difficulty) / _tourlogs.Count);
                            row.RelativeColumn().Text((double)_tourlogs.Sum(tl => tl.HeightDelta) / _tourlogs.Count);
                            row.RelativeColumn().Text(_tourlogs.Sum(tl => tl.MaxSpeed) / _tourlogs.Count);
                        });
                    });
                }); 
            }));
        }

        public DocumentMetadata GetMetadata()
        {
            return DocumentMetadata.Default;
        }
    }
}