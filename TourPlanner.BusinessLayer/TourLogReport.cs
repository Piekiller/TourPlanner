using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using QuestPDF;
using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using TourPlanner.Models;

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
                page.Header().AlignCenter().Text(_tour.Name + " " + _tour.Description); 
                page.Content().AlignRight().Image(File.ReadAllBytes(_tour.RouteInformation));
                page.Content().Stack(stack =>
                {
                    foreach (TourLog log in _tourlogs)
                    {
                        stack.Item().Row(row =>
                        {
                            row.RelativeColumn().Text(log.Difficulty);
                            row.RelativeColumn().Text(log.AvgSpeed);
                            row.RelativeColumn().Text(log.BurnedJoule);
                            row.RelativeColumn().Text(log.Distance);
                            row.RelativeColumn().Text(log.HeightDelta);
                            row.RelativeColumn().Text(log.Rating);
                        });
                    }
                });
            }));
        }

        public DocumentMetadata GetMetadata()
        {
            return DocumentMetadata.Default;
        }
    }
}