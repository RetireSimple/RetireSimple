import {Grid, Typography} from '@mui/material';
import React from 'react';
import {useFormContext} from 'react-hook-form';
import {FormTextField, FormTextFieldMonthUnits} from '../../components/InputComponents';

export const MonteCarloAnalysisForm = () => {
	const formContext = useFormContext();
	const {errors} = formContext.formState;

	//==============================================
	//Field definitions (To reduce indent depth)
	//==============================================
	const analysisLengthField = (
		<FormTextFieldMonthUnits
			name='analysis_analysisLength'
			label='Analysis Length'
			control={formContext.control}
			errorField={errors.analysisLength}
			tooltip='The number of months from today to run the analysis for.'
		/>
	);

	const simCountField = (
		<FormTextField
			name='analysis_simCount'
			label='Simulation Count'
			control={formContext.control}
			errorField={errors.analysis_simCount}
			tooltip={
				<>
					<Typography variant='inherit'>
						The number of Monte Carlo simulations to run.
					</Typography>
					<Typography variant='inherit'>
						Higher numbers are more likely to determine a stable trend with a higher
						resolution, but may take longer to run.
					</Typography>
				</>
			}
		/>
	);

	const randomVariableMuField = (
		<FormTextField
			name='analysis_randomVariableMu'
			label='Mu'
			control={formContext.control}
			errorField={errors.analysis_randomVariableMu}
			tooltip={
				<>
					<Typography variant='inherit'>
						The mean parameter of the random variable.
					</Typography>
					<Typography variant='inherit'>
						Can be considered the average increase in stock price per month.
					</Typography>
					<Typography variant='inherit'>
						This is the statistical analogue of a stock's Alpha value.
					</Typography>
				</>
			}
		/>
	);

	const randomVariableSigmaField = (
		<FormTextField
			name='analysis_randomVariableSigma'
			label='Sigma'
			control={formContext.control}
			errorField={errors.analysis_randomVariableSigma}
			tooltip={
				<>
					<Typography variant='inherit'>
						The standard deviation parameter of the random variable.
					</Typography>
					<Typography variant='inherit'>
						Can be considered the volatility of the stock price.
					</Typography>
					<Typography variant='inherit'>
						This is the statistical analogue of a stock's Beta value.
					</Typography>
				</>
			}
		/>
	);

	const randomVariableScaleFactorField = (
		<FormTextField
			name='analysis_randomVariableScaleFactor'
			label='Scale Factor'
			control={formContext.control}
			errorField={errors.analysis_randomVariableScaleFactor}
			tooltip={
				<>
					<Typography variant='inherit'>
						The scale factor of random variable observations.
					</Typography>
					<Typography variant='inherit'>
						Default is 1, and means that random variable observations are not scaled.
					</Typography>
					<Typography variant='inherit'>
						Useful when Mu is 0 and Sigma is 1, and you want to assert that stock price
						walks are distributed by the random variable.
					</Typography>
				</>
			}
		/>
	);
	return (
		<Grid container spacing={2}>
			<Grid item xs={12}>
				<Typography variant='subtitle2'>Monte Carlo Analysis Parameters</Typography>
			</Grid>
			<Grid item xs={4}>
				{analysisLengthField}
			</Grid>
			<Grid item xs={4}>
				{simCountField}
			</Grid>
			<Grid item xs={12}>
				<Typography variant='subtitle2'>Random Variable Parameters (Normal)</Typography>
			</Grid>
			<Grid item xs={2}>
				{randomVariableMuField}
			</Grid>
			<Grid item xs={2}>
				{randomVariableSigmaField}
			</Grid>
			<Grid item xs={2}>
				{randomVariableScaleFactorField}
			</Grid>
		</Grid>
	);
};
