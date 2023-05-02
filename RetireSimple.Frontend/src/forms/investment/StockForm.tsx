import {Box, Grid, Typography} from '@mui/material';

import React from 'react';
import {useFormContext, useWatch} from 'react-hook-form';
import {
	FormDatePicker,
	FormSelectField,
	FormTextField,
	FormTextFieldCurrency,
	FormTextFieldPercent,
} from '../../components/InputComponents';
import {MonteCarloAnalysisForm} from '../analysis/MonteCarloAnalysisForm';
import {BinomialRegressionAnalysisForm} from '../analysis/BinomialRegressionAnalysisForm';

export interface StockFormProps {
	defaultValues?: any;
	analysisTypeField: React.ReactNode;
}

export const StockForm = (props: StockFormProps) => {
	const formContext = useFormContext();
	const analysisType = useWatch({
		name: 'analysisType',
		control: formContext.control,
		defaultValue: props.defaultValues?.analysisType ?? 'MonteCarlo',
	});

	const {errors} = formContext.formState;

	const analysisSubForm = React.useCallback(() => {
		switch (analysisType) {
			case 'MonteCarlo':
				return <MonteCarloAnalysisForm />;
			case 'BinomialRegression':
				return <BinomialRegressionAnalysisForm />;
			default:
				return (
					<Grid item xs={12}>
						<Typography variant='subtitle2'>
							No analysis parameters available
						</Typography>
					</Grid>
				);
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
			tooltip={
				<>
					<Typography variant='inherit'>The ticker symbol for this stock.</Typography>
					<Typography variant='inherit'>
						This is primarily used as another identifier for the stock.
					</Typography>
				</>
			}
		/>
	);

	const stockPriceField = (
		<FormTextFieldCurrency
			name='stockPrice'
			label='Price'
			control={formContext.control}
			errorField={errors.stockPrice}
			tooltip={
				<>
					<Typography variant='inherit'>The current price of the stock.</Typography>
					<Typography variant='inherit'>
						This does not have to reflect current prices and thus can be changed for
						hypothetical scenarios.
					</Typography>
				</>
			}
		/>
	);

	const stockQuantityField = (
		<FormTextField
			name='stockQuantity'
			label='Quantity'
			control={formContext.control}
			errorField={errors.stockQuantity}
			tooltip={
				<>
					<Typography variant='inherit'>
						The number of shares of this stock that you own.
					</Typography>
					<Typography variant='inherit'>Can be fractional.</Typography>
				</>
			}
		/>
	);

	const stockPurchaseDateField = (
		<FormDatePicker
			name='stockPurchaseDate'
			label='Purchase Date'
			control={formContext.control}
			errorField={errors.stockPurchaseDate}
			defaultValue={props.defaultValues?.stockPurchaseDate}
			tooltip={
				<>
					<Typography variant='inherit'>
						The date that you purchased this stock.
					</Typography>
					<Typography variant='inherit'>
						This is used in dividend calculations.
					</Typography>
				</>
			}
		/>
	);

	const stockDividendPercentField = (
		<FormTextFieldPercent
			name='stockDividendPercent'
			label='Dividend Amount'
			control={formContext.control}
			errorField={errors.stockDividendPercent}
			tooltip={
				<>
					<Typography variant='inherit'>
						The dividend percentage of this stock.
					</Typography>
				</>
			}
		/>
	);

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
			defaultOption='Quarter'
			disable={false}
			tooltip='The interval at which dividends are paid out.'
		/>
	);

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
			tooltip={
				<>
					<Typography variant='inherit'>
						{' '}
						The method in which dividends are paid out.
					</Typography>
					<Typography variant='inherit'>
						Currently, only stock dividends are supported.
					</Typography>
				</>
			}
		/>
	);

	const stockDividendFirstPaymentDateField = (
		<FormDatePicker
			name='stockDividendFirstPaymentDate'
			label='First Payment Date'
			control={formContext.control}
			errorField={errors.stockDividendFirstPaymentDate}
			defaultValue={props.defaultValues?.stockDividendFirstPaymentDate ?? ''}
			tooltip={
				<>
					<Typography variant='inherit'>
						The payment date of the first dividend.
					</Typography>
					<Typography variant='inherit'>
						This can be any date within the past as we predict future dividends based on
						the dividend interval.
					</Typography>
				</>
			}
		/>
	);

	return (
		<Box sx={{flexGrow: 1, marginTop: '1rem'}}>
			<Grid container spacing={2}>
				<Grid item xs={12}>
					<Typography variant='subtitle2'>Stock Information</Typography>
				</Grid>
				<Grid item xs={2}>
					{stockTickerField}
				</Grid>
				<Grid item xs={2}>
					{stockPriceField}
				</Grid>
				<Grid item xs={2}>
					{stockQuantityField}
				</Grid>
				<Grid item xs={4}>
					{stockPurchaseDateField}
				</Grid>
				<Grid item xs={2}></Grid>
				{/* Dividend Section */}
				<Grid item xs={12}>
					<Typography variant='subtitle2'>Dividend Information</Typography>
				</Grid>
				<Grid item xs={2}>
					{stockDividendPercentField}
				</Grid>
				<Grid item xs={2}>
					{stockDividendDistributionIntervalField}
				</Grid>
				<Grid item xs={2}>
					{stockDividendDistributionMethodField}
				</Grid>
				<Grid item xs={4}>
					{stockDividendFirstPaymentDateField}
				</Grid>
				<Grid item xs={2}></Grid>
				{/* Analysis Section */}
				<Grid item xs={12}>
					<Typography variant='subtitle2'>Analysis Configuration</Typography>
				</Grid>
				<Grid item xs={4}>
					{props.analysisTypeField}
				</Grid>

				{analysisSubForm()}
			</Grid>
		</Box>
	);
};
