import {
	FormControl,
	FormHelperText,
	InputAdornment,
	InputLabel,
	MenuItem,
	Select,
	TextField,
	Tooltip,
} from '@mui/material';
import {DatePicker, LocalizationProvider} from '@mui/x-date-pickers';
import {AdapterDayjs} from '@mui/x-date-pickers/AdapterDayjs';
import React from 'react';
import {Control, Controller} from 'react-hook-form';

export interface FormTextFieldProps {
	name: string;
	label: string;
	control: Control;
	errorField: any;
	tooltip?: string;
	decoration?: 'currency' | 'percent';
}

export interface FormSelectFieldProps {
	name: string;
	label: string;
	options: {value: string; label: string}[];
	defaultOption: string; //Assert it is in options for easy programming
	control: Control;
	errorField: any;
	disable: boolean;
	tooltip?: string;
}

export interface FormDatePickerProps {
	name: string;
	label: string;
	control: Control;
	errorField: any;
	defaultValue: string;
	tooltip?: string;
}

export const FormTextField = (props: FormTextFieldProps) => {
	return (
		<Controller
			name={props.name}
			control={props.control}
			defaultValue={''}
			render={({field}) => (
				<Tooltip title={props.tooltip ?? ''} arrow>
					<TextField
						{...field}
						label={props.label}
						fullWidth
						size='small'
						error={!!props.errorField}
						helperText={props.errorField?.message as string}
						InputProps={{
							startAdornment: props.decoration === 'currency' && (
								<InputAdornment position='start'>$</InputAdornment>
							),
						}}
					/>
				</Tooltip>
			)}
		/>
	);
};

export const FormSelectField = (props: FormSelectFieldProps) => {
	return (
		<Controller
			name={props.name}
			control={props.control}
			defaultValue={props.defaultOption}
			render={({field}) => (
				<Tooltip title={props.tooltip ?? ''} arrow>
					<FormControl fullWidth error={!!props.errorField} size='small'>
						<InputLabel id={props.name}>{props.label}</InputLabel>
						<Select {...field} label={props.name} disabled={props.disable}>
							{props.options.map((option) => (
								<MenuItem key={option.value} value={option.value}>
									{option.label}
								</MenuItem>
							))}
						</Select>
						<FormHelperText>{props.errorField?.message as string}</FormHelperText>
					</FormControl>
				</Tooltip>
			)}
		/>
	);
};

export const FormDatePicker = (props: FormDatePickerProps) => {
	return (
		<Controller
			name={props.name}
			control={props.control}
			defaultValue={props.defaultValue ?? ''}
			render={({field}) => (
				<LocalizationProvider dateAdapter={AdapterDayjs}>
					<Tooltip title={props.tooltip ?? ''} arrow>
						<DatePicker
							{...field}
							label={props.label}
							renderInput={(params) => (
								<TextField
									{...params}
									size='small'
									error={!!props.errorField}
									helperText={props.errorField?.message as string}
								/>
							)}
						/>
					</Tooltip>
				</LocalizationProvider>
			)}
		/>
	);
};
