import React, { useEffect, useState } from 'react';
import { Bar } from 'react-chartjs-2';
import {
  Chart as ChartJS,
  CategoryScale,
  LinearScale,
  BarElement,
  Title,
  Tooltip,
  Legend,
} from 'chart.js';
import './WeatherGraph.css';

ChartJS.register(CategoryScale, LinearScale, BarElement, Title, Tooltip, Legend);

interface WeatherRecord {
  id: number;
  country: string;
  city: string;
  temperature: number;
  minTemperature: number;
  maxTemperature: number;
  lastUpdate: string;
}

interface CityTempStat {
  city: string;
  country: string;
  min: number;
  max: number;
}

const WeatherGraph: React.FC = () => {
  const [stats, setStats] = useState<CityTempStat[]>([]);

  useEffect(() => {
    fetch(
      'https://localhost:7074/api/Weather/GetLogs'
    )
      .then((res) => res.json())
      .then((data: WeatherRecord[]) => {
        const computedStats: CityTempStat[] = [];
        data.forEach((record) => {
          const computedStat : CityTempStat = {
            city: record.city,
            country: record.country,
            min: record.minTemperature,
            max: record.maxTemperature
          }

          computedStats.push(computedStat);
        });
      
        //WASN'T SURE IF I NEED TO SHOW MIN/MAX FROM THE DATA, OR CALCULATE IT SO BOTH HERE
        
        // const grouped: { [key: string]: WeatherRecord[] } = {};
        // data.forEach((record) => {
        //   const key = `${record.country}-${record.city}`;
        //   if (!grouped[key]) {
        //     grouped[key] = [];
        //   }
        //   grouped[key].push(record);
        // });

        // const computedStats: CityTempStat[] = Object.keys(grouped).map((key) => {
        //   const records = grouped[key];
        //   const temperatures = records.map((r) => r.temperature);
        //   const min = Math.min(...temperatures);
        //   const max = Math.max(...temperatures);
        //   const [country, city] = key.split('-');
        //   return { city, country, min, max };
        // });

        setStats(computedStats);
      })
      .catch((err) => console.error('Error fetching weather logs:', err));
  }, []);

  // Prepare the chart data
  const data = {
    labels: stats.map((s) => `${s.city} (${s.country})`),
    datasets: [
      {
        label: 'Min Temperature (°C)',
        data: stats.map((s) => s.min),
        backgroundColor: 'rgba(75, 192, 192, 0.6)',
      },
      {
        label: 'Max Temperature (°C)',
        data: stats.map((s) => s.max),
        backgroundColor: 'rgba(153, 102, 255, 0.6)',
      },
    ],
  };

  const chartOptions = {
    responsive: true,
    plugins: {
      legend: {
        position: 'top' as const,
      },
      title: {
        display: true,
        text: 'Min and Max Temperatures by City',
      },
    },
  };

  return (
    <div className="bar-div">
      <Bar data={data} options={chartOptions} />
    </div>
  );
};

export default WeatherGraph;