import {Grid, Typography} from '@mui/material';
import React from 'react';
import {useFormContext} from 'react-hook-form';
import {FormTextField} from '../../components/InputComponents';

export const MonteCarloAnalysisForm = () => {
	const formContext = useFormContext();
	const {errors} = formContext.formState;

	//==============================================
	//Field definitions (To reduce indent depth)
	//==============================================
	const analysisLengthField = (
		<FormTextField
			name='analysis_analysisLength'
			label='Analysis Length (Months)'
			control={formContext.control}
			errorField={errors.analysisLength}
		/>
	);

	const simCountField = (
		<FormTextField
			name='analysis_simCount'
			label='Simulation Count'
			control={formContext.control}
			errorField={errors.analysis_simCount}
		/>
	);

	const randomVariableMuField = (
		<FormTextField
			name='analysis_randomVariableMu'
			label='Mu'
			control={formContext.control}
			errorField={errors.analysis_randomVariableMu}
		/>
	);

	const randomVariableSigmaField = (
		<FormTextField
			name='analysis_randomVariableSigma'
			label='Sigma'
			control={formContext.control}
			errorField={errors.analysis_randomVariableSigma}
		/>
	);

	const randomVariableScaleFactorField = (
		<FormTextField
			name='analysis_randomVariableScaleFactor'
			label='Scale Factor'
			control={formContext.control}
			errorField={errors.analysis_randomVariableScaleFactor}
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
