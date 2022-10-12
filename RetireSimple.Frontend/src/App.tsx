import React from 'react';


export interface Forecast {
    date: string;
    temperatureC: number;
    temperatureF: number;
    summary?: string;
}

export default function App() {
    const [forecasts, setForecasts] = React.useState<Forecast[]>([]);
    const [loading, setLoading] = React.useState<boolean>(true);



    const populateWeatherData = async () => {
        const response = await fetch('weatherforecast');
        const data = await response.json();
        setForecasts(data);
        setLoading(false);
    };

    React.useEffect(() => {
        if (loading) { populateWeatherData() };
    });

    const renderForecastsTable = (forecasts: Forecast[]) => {
        return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                    <tr>
                        <th>Date</th>
                        <th>Temp. (C)</th>
                        <th>Temp. (F)</th>
                        <th>Summary</th>
                    </tr>
                </thead>
                <tbody>
                    {forecasts.map(forecast =>
                        <tr key={forecast.date}>
                            <td>{forecast.date}</td>
                            <td>{forecast.temperatureC}</td>
                            <td>{forecast.temperatureF}</td>
                            <td>{forecast.summary}</td>
                        </tr>
                    )}
                </tbody>
            </table>
        );
    }

    {
        let contents = loading
            ? <p><em>Loading... Please refresh once the ASP.NET backend has started. See <a href="https://aka.ms/jspsintegrationreact">https://aka.ms/jspsintegrationreact</a> for more details.</em></p>
            : renderForecastsTable(forecasts);

        return (
            <div>
                <h1 id="tabelLabel" >Weather forecast</h1>
                <p>This component demonstrates fetching data from the server.</p>
                {contents}
            </div>

        );

    }
}
