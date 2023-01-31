import React from 'react';
import {FieldValues, FormProvider, useForm} from 'react-hook-form';
import {InvestmentDataForm} from '../forms/InvestmentDataForm';
import {useLoaderData} from 'react-router-dom';
import {Investment} from '../data/Interfaces';
import {Typography} from '@mui/material';
import {yupResolver} from '@hookform/resolvers/yup';
import {investmentFormSchema} from '../data/FormSchema';

export const InvestmentView = () => {

	const currentInvestmentData = useLoaderData() as Investment

	const investmentDataFormContext = useForm({
		shouldUnregister:true,
		resolver: yupResolver(investmentFormSchema),
		defaultValues: currentInvestmentData as FieldValues});


	// let chart = renderAnalysis();

	return (
		<>
			<FormProvider {...investmentDataFormContext}>
				<InvestmentDataForm onSubmit={(data: FieldValues) => {console.log(data);}} />
			</FormProvider>

			<Typography variant='h4'>Router Data (Testing Only)</Typography>
			<Typography variant='body1'>{JSON.stringify(currentInvestmentData)}</Typography>
			{/*
					<input type="text" onChange={e => setInvestmentModelId(e.target.value)}></input>
					<button onClick={getAnalysis}>Get Analysis Model</button>

					{chart} */}
		</>
	);
};
