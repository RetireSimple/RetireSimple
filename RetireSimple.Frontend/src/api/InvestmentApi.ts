import {Investment, InvestmentModel} from '../data/Interfaces';

export const API_BASE_URL = 'https://localhost:3000/api';

export const getInvestmentModel = async (id: number): Promise<InvestmentModel> => {
	const response = await fetch(`${API_BASE_URL}/Analysis/GetAnalysis?investmentID=${id}`, {
		method: 'POST',
		headers: {
			'Content-Type': 'application/json',
		},
	});

	return await response.json();
};

export const getInvestments = async (): Promise<Investment[]> => {
	const response = await fetch(`${API_BASE_URL}/Investment/GetAllInvestments`);
	return await response.json();
};

export const getInvestment = async (id: number): Promise<Investment> => {
	const response = await fetch(`${API_BASE_URL}/Investment/GetInvestment/${id}`);
	return await response.json();
};

export const addStock = async (stock: any) => {

	await fetch(`${API_BASE_URL}/Investment/AddStock`, {
		method: 'POST',
		body: JSON.stringify(stock),
		headers: {
			Accept: 'application/json',
			'Content-Type': 'application/json; charset=utf-8',
		},
	});
};
