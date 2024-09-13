using System;
using System.Collections.Generic;
using System.Text;

namespace ContactCenter.Lib
{
    public enum TicketHeading : int
    {
        ReportFireIncident = 1,
        ReportDisaster = 2,
        ReportAccident = 3,
        RequestAssistance = 4,
        TrackEmergencyReport = 5,
        ReportWaterLeak = 6,
        NoWaterSupply = 7,
        WaterQualityConcerns = 8,
        WaterBillingQueries = 9,
        TrackResolution = 10,
        ViewEmergencyContacts = 11,
        ReportFraud = 12,
        ReportCorruption = 13,
        RequestAmbulance = 14,
        Crime = 15,
        RiotAlert = 16,
        Other = 17
    }
}
