import {Investment, InvestmentModel} from '../data/Interfaces';

export const API_BASE_URL = process.env.REACT_APP_API_URL;

export const getInvestmentModel = async (id: number): Promise<InvestmentModel> => {
	const response = await fetch(`${API_BASE_URL}/Analysis/Investment/${id}`, {
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

export const addInvestment = async (data: any) => {
	await fetch(`${API_BASE_URL}/Investment/Add`, {
		method: 'POST',
		body: JSON.stringify(data),
		headers: {
			Accept: 'application/json',
			'Content-Type': 'application/json; charset=utf-8',
		},
	});
};

export const deleteInvestment = async (id: number) => {
	await fetch(`${API_BASE_URL}/Investment/Delete/${id}`, {
		method: 'DELETE',
	});
};

export const updateInvestment = async (id: number, data: any) => {
	await fetch(`${API_BASE_URL}/Investment/Update/${id}`, {
		method: 'POST',
		body: JSON.stringify(data),
		headers: {
			Accept: 'application/json',
			'Content-Type': 'application/json; charset=utf-8',
		},
	});
};
