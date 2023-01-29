import {Investment, InvestmentModel} from '../models/Interfaces';

export const API_BASE_URL = 'https://localhost:3000/api';

export const getInvestmentModel = async (id: number): Promise<InvestmentModel> => {
	const response = await fetch(`${API_BASE_URL}/Analysis/GetAnalysis?investmentID=${id}`, {
		method: 'GET',
		headers: {
			'Content-Type': 'application/json',
		},
	});

	return await response.json();
};

export const getInvestments = async (): Promise<Investment[]> => {
	const response = await fetch('/api/Investment/GetAllInvestments');
	return await response.json();
};

export const addStock = async (stock: any) => {
	await fetch('/api/Investment/AddStock', {
		method: 'POST',
		body: JSON.stringify(stock),
		headers: {
			Accept: 'application/json',
			'Content-Type': 'application/json; charset=utf-8',
		},
	});
};