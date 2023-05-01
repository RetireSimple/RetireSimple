import {ApiExpense, FullModelData, Investment, InvestmentModel} from '../Interfaces';
import {API_BASE_URL} from './ApiCommon';
import {convertDates, convertToDecimal} from './ConvertUtils';

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
	const response = await fetch(`${API_BASE_URL}/Investment`);
	return await response.json();
};

export const getInvestment = async (id: number): Promise<Investment> => {
	const response = await fetch(`${API_BASE_URL}/Investment/${id}`);
	return await response.json();
};

export const addInvestment = async (data: any): Promise<string> => {
	convertToDecimal(data);
	const response = await fetch(`${API_BASE_URL}/Investment`, {
		method: 'POST',
		body: JSON.stringify(data),
		headers: {
			Accept: 'application/json',
			'Content-Type': 'application/json; charset=utf-8',
		},
	});

	return response.text();
};

export const deleteInvestment = async (id: number) => {
	await fetch(`${API_BASE_URL}/Investment/${id}`, {
		method: 'DELETE',
	});
};

export const updateInvestment = async (id: number, data: any) => {
	convertToDecimal(data);
	await fetch(`${API_BASE_URL}/Investment/${id}`, {
		method: 'POST',
		body: JSON.stringify(data),
		headers: {
			Accept: 'application/json',
			'Content-Type': 'application/json; charset=utf-8',
		},
	});
};

export const getAggregateModel = async (): Promise<FullModelData> => {
	const portfolioResponse = await fetch(`${API_BASE_URL}/Analysis/Portfolio`, {
		method: 'POST',
		headers: {
			'Content-Type': 'application/json',
		},
	});
	const invModelsResponse = await fetch(`${API_BASE_URL}/Analysis/Investment`, {
		method: 'POST',
		headers: {'Content-Type': 'application/json'},
	});

	const result: FullModelData = {
		portfolioModel: await portfolioResponse.json(),
		investmentModels: await invModelsResponse.json(),
	};

	return result;
};

export const getExpenses = async (id: number): Promise<ApiExpense[]> => {
	const response = await fetch(`${API_BASE_URL}/Expenses/${id}`);
	return await response.json();
};

export const addExpense = async (data: any) => {
	convertToDecimal(data);
	convertDates(data);
	await fetch(`${API_BASE_URL}/Expenses`, {
		method: 'POST',
		body: JSON.stringify(data),
		headers: {
			Accept: 'application/json',
			'Content-Type': 'application/json; charset=utf-8',
		},
	});
};

export const deleteExpense = async (id: number) => {
	await fetch(`${API_BASE_URL}/Expenses/${id}`, {
		method: 'DELETE',
	});
};
