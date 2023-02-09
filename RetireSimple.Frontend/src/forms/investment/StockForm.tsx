import {Box, FormControl, Grid, InputLabel, MenuItem, Select, TextField, Typography} from '@mui/material';
import {DatePicker, LocalizationProvider} from '@mui/x-date-pickers';
import {AdapterDayjs} from '@mui/x-date-pickers/AdapterDayjs';

import React from 'react';
import {Controller, FormProvider, useFormContext, useWatch} from 'react-hook-form';
import {FormDatePicker, FormSelectField, FormTextField} from '../Inputs';
import {MonteCarloAnalysisForm} from '../analysis/MonteCarloAnalysisForm';

//Type aliases to shorten names

export interface StockFormProps {
	defaultValues?: any;
}

export const StockForm = (props: StockFormProps) => {
	const formContext = useFormContext();
	const analysisType = useWatch({
		name: 'analysisType',
		control: formContext.control,
		defaultValue: props.defaultValues?.analysisType ?? 'MonteCarlo_NormalDist'});

	const {errors} = formContext.formState;

	const analysisSubForm = React.useCallback(() => {
		switch (analysisType) {
		case 'MonteCarlo_NormalDist':
		case 'MonteCarlo_LogNormalDist':
			return <MonteCarloAnalysisForm />;
		default:
			return <Typography variant='subtitle2'>No analysis parameters available</Typography>;
		}
	}, [analysisType]);

	//==============================================
	//Field definitions (To reduce indent depth)
	//==============================================
	const stockTickerField = (
		<FormTextField
			name='stockTicker'
			label='Ticker'
			control={formContext.control}
			errorField={errors.stockTicker}
		/>);

	const stockPriceField = (
		<FormTextField
			name='stockPrice'
			label='Price'
			control={formContext.control}
			errorField={errors.stockPrice}
		/>);

	const stockQuantityField = (
		<FormTextField
			name='stockQuantity'
			label='Quantity'
			control={formContext.control}
			errorField={errors.stockQuantity}
		/>);

	const stockPurchaseDateField = (
		<Controller
			name='stockPurchaseDate'
			control={formContext.control}
			defaultValue={props.defaultValues?.stockPurchaseDate ?? ''}
			render={({field}) => (
				<LocalizationProvider dateAdapter={AdapterDayjs}>
					<DatePicker
						{...field}
						label='Purchase Date'
						renderInput={(params) => <TextField {...params} size='small' />}
					/>
				</LocalizationProvider>
			)} />);

	const stockDividendPercentField = (
		<FormTextField
			name='stockDividendPercent'
			label='Dividend %'
			control={formContext.control}
			errorField={errors.stockDividendPercent}
		/>);

	const stockDividendDistributionIntervalField = (
		<FormSelectField
			name='stockDividendDistributionInterval'
			label='Dividend Int.'
			control={formContext.control}
			errorField={errors.stockDividendDistributionInterval}
			options={[
				{value: 'Month', label: 'Monthly'},
				{value: 'Quarter', label: 'Quarterly'},
				{value: 'Annual', label: 'Annual'},
			]}
			defaultOption='Month'
		/>);


	const stockDividendDistributionMethodField = (
		<FormSelectField
			name='stockDividendDistributionMethod'
			label='Dividend Method'
			control={formContext.control}
			errorField={errors.stockDividendDistributionMethod}
			options={[
				{value: 'Stock', label: 'Stock'},
				{value: 'Cash', label: 'Cash'},
				{value: 'DRIP', label: 'DRIP'},
			]}
			defaultOption='Stock'
		/>);

	const stockDividendFirstPaymentDateField = (
		<FormDatePicker
			name='stockDividendFirstPaymentDate'
			label='First Payment Date'
			control={formContext.control}
			errorField={errors.stockDividendFirstPaymentDate}
			defaultValue={props.defaultValues?.stockDividendFirstPaymentDate ?? ''}
		/>);


	const analysisTypeField = (
		<Controller
			name='analysisType'
			control={formContext.control}
			defaultValue={"MonteCarlo_NormalDist"}
			render={({field}) => (
				<FormControl fullWidth size='small'>
					<InputLabel id='analysisType'>Analysis Type</InputLabel>
					<Select {...field}
						label='Analysis Type'>
						<MenuItem value='testAnalysis'>Test Analysis</MenuItem>
						<MenuItem value='MonteCarlo_NormalDist'>Monte Carlo - Normal Dist.</MenuItem>
						<MenuItem value='MonteCarlo_LogNormalDist'>Monte Carlo - Log Normal Dist.</MenuItem>
					</Select>
				</FormControl>
			)} />);

	return (
		<FormProvider {...formContext}>
			<Box sx={{flexGrow: 1, marginTop: '1rem'}}>
				<Typography variant='subtitle2'>Stock Information</Typography>
				<Grid container spacing={2}>
					<Grid item xs={2}>{stockTickerField}</Grid>
					<Grid item xs={2}>{stockPriceField}</Grid>
					<Grid item xs={2}>{stockQuantityField}</Grid>
					<Grid item xs={4}>{stockPurchaseDateField}</Grid>
					<Grid item xs={2}></Grid>
					{/* Dividend Section */}
					<Grid item xs={12}>
						<Typography variant='subtitle2'>Dividend Information</Typography>
					</Grid>
					<Grid item xs={2}>{stockDividendPercentField}</Grid>
					<Grid item xs={2}>{stockDividendDistributionIntervalField}</Grid>
					<Grid item xs={2}>{stockDividendDistributionMethodField}</Grid>
					<Grid item xs={4}>{stockDividendFirstPaymentDateField}</Grid>
					<Grid item xs={2}></Grid>
					{/* Analysis Section */}
					<Grid item xs={12}>
						<Typography variant='subtitle2'>Analysis Configuration</Typography>
					</Grid>
					<Grid item xs={4}>{analysisTypeField}</Grid>
					<Grid item xs={12}>
						{analysisSubForm()}
					</Grid>
				</Grid>
			</Box>
		</FormProvider>
	);
};
