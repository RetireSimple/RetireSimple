import {yupResolver} from '@hookform/resolvers/yup';
import {Box, Button, Dialog, DialogActions, DialogTitle} from '@mui/material';
import {FieldValues, FormProvider, useForm} from 'react-hook-form';
import {useNavigate} from 'react-router-dom';
import {investmentFormSchema} from '../../data/FormSchema';
import {InvestmentDataForm} from '../../forms/InvestmentDataForm';

// export interface AddInvestmentDialogProps {
// 	onClose: () => void;
// }

export const AddInvestmentDialog = () => {
	const formContext = useForm({
		shouldUnregister: true,
		resolver: yupResolver(investmentFormSchema)});

	const navigate = useNavigate();

	const handleClose = () => {
		navigate(-1);
	};

	const onSubmit = (data: FieldValues) => {
		console.log(data);
		handleClose();
	};

	const handleAdd = () => {
		formContext.trigger().then((isValid) => {
			if (isValid) {
				const data = formContext.getValues();
				console.log(data);
				handleClose();
			}
		}).catch((err) => {
			console.log(err);
		});

	};

	return (
		<FormProvider {...formContext}>
			<Dialog open
				maxWidth='md'>
				<DialogTitle>Add Investment</DialogTitle>
				<Box sx={{padding: '2rem'}}>
					<InvestmentDataForm onSubmit={onSubmit} />
				</Box>
				<DialogActions>
					<Button onClick={handleClose}>Cancel</Button>
					<Button onClick={handleAdd}>Add</Button>
				</DialogActions>
			</Dialog>
		</FormProvider>
	);
};
