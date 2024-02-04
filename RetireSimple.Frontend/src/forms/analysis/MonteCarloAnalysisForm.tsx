import {FormControlLabel, Grid, Switch, Typography} from '@mui/material';
import React from 'react';
import {useFormContext, useWatch} from 'react-hook-form';
import {PresetContext} from '../../Layout';
import {addSpacesCapitalCase} from '../../api/ConvertUtils';
import {
	FormSelectField,
	FormTextField,
	FormTextFieldMonthUnits,
} from '../../components/InputComponents';

export interface MonteCarloProps {
	analysisLength: string;
	analysisPreset: string;
}

export const MonteCarloAnalysisForm = (props: MonteCarloProps) => {
	const formContext = useFormContext();
	const {errors} = formContext.formState;
	const presets = React.useContext(PresetContext)?.['MonteCarlo'];
	const currentPreset = useWatch({
		control: formContext.control,
		name: 'analysis_analysisPreset',
		defaultValue: formContext.getValues('analysis_analysisPreset'),
	});
	const [showSettings, setShowSettings] = React.useState(false);
	const [showAdvanced, setShowAdvanced] = React.useState(false);

	React.useEffect(() => {
		setShowAdvanced(currentPreset === 'Custom');
	}, [currentPreset]);

	const presetOptions = React.useMemo(() => {
		if (!presets)
			return [{label: 'Custom', value: 'Custom'}] as {label: string; value: string}[];
		const presetList: {label: string; value: string}[] = Object.keys(presets).map(
			(presetName) => ({
				label: addSpacesCapitalCase(presetName),
				value: presetName,
			}),
		);

		presetList.push({label: 'Custom', value: 'Custom'});
		return presetList;
	}, [presets]);

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
			defaultValue={props.analysisLength}		/>
	);

	const analysisPresetField = (
		<FormSelectField
			name='analysis_analysisPreset'
			label='Analysis Preset'
			control={formContext.control}
			errorField={errors.analysis_analysisPreset}
			tooltip='A preset to use for the analysis.'
			options={presetOptions}
			defaultOption={props.analysisPreset}
			disable={false}
		/>
	);

	const simCountField = (
		<FormTextField
			name='analysis_simCount'
			label='Simulation Count'
			control={formContext.control}
			errorField={errors.analysis_simCount}
			tooltip={<>
				<Typography variant='inherit'>
					The number of Monte Carlo simulations to run.
				</Typography>
				<Typography variant='inherit'>
					Higher numbers are more likely to determine a stable trend with a higher
					resolution, but may take longer to run.
				</Typography>
			</>} defaultValue={''}		/>
	);

	const randomVariableTypeField = (
		<FormSelectField
			name='analysis_randomVariableType'
			label='Random Variable Type'
			control={formContext.control}
			errorField={errors.analysis_randomVariableType}
			tooltip={
				<>
					<Typography variant='inherit'>
						The type of random variable to use for the simulation.
					</Typography>
					<Typography variant='inherit'>
						Normal is the most common, but can be changed to other distributions.
					</Typography>
				</>
			}
			options={[
				{label: 'Normal', value: 'Normal', tooltip: 'Normal'},
				{label: 'Log Normal', value: 'LogNormal', tooltip: 'Log Normal'},
			]}
			defaultOption={'Normal'}
			disable={false}
		/>
	);

	const randomVariableMuField = (
		<FormTextField
			name='analysis_randomVariableMu'
			label='Mu'
			control={formContext.control}
			errorField={errors.analysis_randomVariableMu}
			tooltip={<>
				<Typography variant='inherit'>
					The mean parameter of the random variable.
				</Typography>
				<Typography variant='inherit'>
					Can be considered the average increase in stock price per month.
				</Typography>
				<Typography variant='inherit'>
					This is the statistical analogue of a stock's Alpha value.
				</Typography>
			</>} defaultValue={''}		/>
	);

	const randomVariableSigmaField = (
		<FormTextField
			name='analysis_randomVariableSigma'
			label='Sigma'
			control={formContext.control}
			errorField={errors.analysis_randomVariableSigma}
			tooltip={<>
				<Typography variant='inherit'>
					The standard deviation parameter of the random variable.
				</Typography>
				<Typography variant='inherit'>
					Can be considered the volatility of the stock price.
				</Typography>
				<Typography variant='inherit'>
					This is the statistical analogue of a stock's Beta value.
				</Typography>
			</>} defaultValue={''}		/>
	);

	const randomVariableScaleFactorField = (
		<FormTextField
			name='analysis_randomVariableScaleFactor'
			label='Scale Factor'
			control={formContext.control}
			errorField={errors.analysis_randomVariableScaleFactor}
			tooltip={<>
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
			</>} defaultValue={''}		/>
	);

	const getPresetValue = (field: string) => {
		if (!presets || !currentPreset || !Object.keys(presets).includes(currentPreset))
			return 'Not Set';
		return presets[currentPreset][field];
	};

	return (
		<>
			<Grid item xs={4}>
				{analysisLengthField}
			</Grid>
			<Grid item xs={4}>
				{analysisPresetField}
			</Grid>
			{!showAdvanced && (
				<>
					<Grid item xs={4}>
						<FormControlLabel
							label='What Is This Analysis?'
							control={
								<Switch
									checked={showSettings}
									onChange={() => setShowSettings(!showSettings)}
								/>
							}
						/>
					</Grid>
					{showSettings && (
						<Grid item xs={12}>
							<Typography variant='subtitle2'>
								Summary of This Analysis Method
							</Typography>
							<Typography variant='body2'>
								{`A Monte Carlo Simulation (also referred to as a method), is a type of algorithm where results are obtained after
								 a large number of trials that invlove some form of randomness. The idea behind a Monte Carlo simulation is
								 that by the Law of Large Numbers, running more trials will increase the accuracy of results (such as the
								 average value) determined from those trials. In this case, the trials are based on the "random walk" of
								 a stock price with an asserted average increase and volatility. It assumes the random walk of the stock
								 price is "normally or log-normally distributed" and for each trial adds a random value to the stock price
								  by "taking a sample of the random walk" for each month to model.`}
							</Typography>
							<br />
							<Typography variant='body2'>
								Monte Carlo Simulations tend to be computationally intensive, even
								with optimizations. Default presets use around 500,000 trials, which
								is a good balance between accuracy and speed.
							</Typography>
							<br />
							<Typography variant='subtitle2'>Monte Carlo Parameters</Typography>
							<Typography variant='body2'>
								{`Simulations to Run (Number of Trials): ${getPresetValue(
									'analysis_simCount',
								)}`}
							</Typography>
							<Typography variant='body2'>
								{`Random Variable Type:	${getPresetValue(
									'analysis_randomVariableType',
								)}`}
							</Typography>
							<Typography variant='body2'>
								{`Random Variable Mu: ${getPresetValue(
									'analysis_randomVariableMu',
								)}`}
							</Typography>
							<Typography variant='body2'>
								{`Random Variable Sigma: ${getPresetValue(
									'analysis_randomVariableSigma',
								)}`}
							</Typography>
							<Typography variant='body2'>
								{`Random Variable Scale Factor:	${getPresetValue(
									'analysis_randomVariableScaleFactor',
								)}`}
							</Typography>
						</Grid>
					)}
				</>
			)}
			{showAdvanced && (
				<>
					<Grid item xs={12}>
						<Typography variant='subtitle2'>Custom Monte Carlo Parameters</Typography>
					</Grid>
					<Grid item xs={4}>
						{simCountField}
					</Grid>
					<Grid item xs={4}>
						{randomVariableTypeField}
					</Grid>
					<Grid item xs={12}>
						<Typography variant='subtitle2'>Random Variable Parameters</Typography>
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
				</>
			)}
		</>
	);
};
