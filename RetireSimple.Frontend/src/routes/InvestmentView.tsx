import {yupResolver} from '@hookform/resolvers/yup';
import React from 'react';
import {FieldValues, FormProvider, useForm} from 'react-hook-form';
import {Outlet, useLoaderData} from 'react-router-dom';
import {investmentFormSchema} from '../data/FormSchema';
import {Investment} from '../data/Interfaces';
import {InvestmentDataForm} from '../forms/InvestmentDataForm';

export const InvestmentView = () => {

	const currentInvestmentData = useLoaderData() as Investment

	const investmentDataFormContext = useForm({
		shouldUnregister:true,
		resolver: yupResolver(investmentFormSchema),
		defaultValues: currentInvestmentData as FieldValues});
	const {reset} = investmentDataFormContext;

	React.useEffect(() => {
		reset(currentInvestmentData);
	// eslint-disable-next-line react-hooks/exhaustive-deps
	}, [currentInvestmentData]);

	return (
		<>
			<FormProvider {...investmentDataFormContext}>
				<InvestmentDataForm />
			</FormProvider>

			{/* <Typography variant='h4'>Router Data (Testing Only)</Typography> */}
			{/* <Typography variant='body1'>{JSON.stringify(currentInvestmentData)}</Typography> */}
			{/*
					<input type="text" onChange={e => setInvestmentModelId(e.target.value)}></input>
					<button onClick={getAnalysis}>Get Analysis Model</button>

					{chart} */}
			<Outlet />
		</>
	);
};
