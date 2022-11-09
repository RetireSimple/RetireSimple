import React from 'react';
import { Investment, InvestmentModel } from './Models/Interfaces';


export interface Forecast {
    date: string;
    temperatureC: number;
    temperatureF: number;
    summary?: string;
}



export default function App() {
    //const [forecasts, setForecasts] = React.useState<Forecast[]>([]);
    const [investments, setInvestments] = React.useState<Investment[]>([]);
    //const [investmentModels, setInvestmentModels] = React.useState<InvestmentModel[]>([]);
    const [loading, setLoading] = React.useState<boolean>(true);



    const populateInvestmentData = async () => {
        const response = await fetch('/api/Investment/GetAllInvestments');
        const data = await response.json();
        setInvestments(data);
        setLoading(false);
    };

    const addNewInvestment = async () => {
        const request = new Request('/api/Investment/AddRandomStock', { method: 'POST' })

        fetch(request).then(async () =>
            await populateInvestmentData());
    }

    React.useEffect(() => {
        if (loading) { populateInvestmentData() };
    });

    const renderInvestmentsTable = (investments: Investment[]) => {
        return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                    <tr>
                        <th>ID</th>
                        <th>Type</th>
                        <th>Analysis Type</th>
                        <th>Last Analysis</th>
                        <th>Ticker</th>
                        <th>Price</th>
                        <th>Quantity</th>
                        <th>Purchase Date</th>
                    </tr>
                </thead>
                <tbody>
                    {investments.map(inv =>
                        <tr key={inv.investmentId}>
                            <td>{inv.investmentId}</td>
                            <td>{inv.investmentType}</td>
                            <td>{inv.analysisType}</td>
                            <td>{inv.lastAnalysis}</td>
                            <td>{inv.investmentData['stockTicker']}</td>
                            <td>{inv.investmentData['stockPrice']}</td>
                            <td>{inv.investmentData['stockQuantity']}</td>
                            <td>{inv.investmentData['stockPurchaseDate']}</td>
                        </tr>
                    )}
                </tbody>
            </table>
        );
    }

    {
        let contents = loading
            ? <p><em>Loading... Please refresh once the ASP.NET backend has started. See <a href="https://aka.ms/jspsintegrationreact">https://aka.ms/jspsintegrationreact</a> for more details.</em></p>
            : renderInvestmentsTable(investments);

        return (
            <div>
                <button onClick={addNewInvestment}>Add New Stonk</button>
                <h1 id="tabelLabel" >Investments</h1>
                <p></p>


                {contents}
            </div>

        );

    }
}
