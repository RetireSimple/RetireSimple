import {Portfolio} from "../Interfaces";

export const API_BASE_URL = import.meta.env.VITE_API_URL;

export const getPortfolio = async (): Promise<Portfolio> => {
	const response = await fetch(`${API_BASE_URL}/Portfolio`);
	const data = await response.json();
	return data;
};
