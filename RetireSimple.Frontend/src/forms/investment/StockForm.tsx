import {Box, Grid, Typography} from '@mui/material';

import React from 'react';
import {useFormContext, useWatch} from 'react-hook-form';
import {FormDatePicker, FormSelectField, FormTextField} from '../Inputs';
import {MonteCarloAnalysisForm} from '../analysis/MonteCarloAnalysisForm';

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
		<FormDatePicker
			name='stockPurchaseDate'
			label='Purchase Date'
			control={formContext.control}
			errorField={errors.stockPurchaseDate}
			defaultValue={props.defaultValues?.stockPurchaseDate}
		/>);

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
			disable={false}
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
			disable={false}
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
		<FormSelectField
			name='analysisType'
			label='Analysis Type'
			control={formContext.control}
			errorField={errors.analysisType}
			options={[
				{value: 'MonteCarlo_NormalDist', label: 'Monte Carlo (Normal Dist)'},
				{value: 'MonteCarlo_LogNormalDist', label: 'Monte Carlo (Log Normal Dist)'},
			]}
			defaultOption='MonteCarlo_NormalDist'
			disable={false}
		/>);


	return (
		<Box sx={{flexGrow: 1, marginTop: '1rem'}}>
			<Grid container spacing={2}>
				<Grid item xs={12}>
					<Typography variant='subtitle2'>Stock Information</Typography>
				</Grid>
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
	);
};
