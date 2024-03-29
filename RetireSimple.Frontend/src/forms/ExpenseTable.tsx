import {
	Box,
	Button,
	Icon,
	IconButton,
	Paper,
	Table,
	TableBody,
	TableCell,
	TableContainer,
	TableFooter,
	TableHead,
	TablePagination,
	TableRow,
} from '@mui/material';
import React from 'react';
import {ApiExpense} from '../Interfaces';
import {deleteExpense, getExpenses} from '../api/InvestmentApi';
import {AddExpenseDialog} from '../components/DialogComponents';
import TablePaginationActions from '@mui/material/TablePagination/TablePaginationActions';
import {useFormAction, useSubmit} from 'react-router-dom';
import {useSnackbar} from 'notistack';

interface ExpensesTableProps {
	investmentId: number;
}

export const ExpensesTable = (props: ExpensesTableProps) => {
	const [expenses, setExpenses] = React.useState<ApiExpense[]>([]);
	const [addDialogOpen, setAddDialogOpen] = React.useState(false);
	const [needsUpdate, setNeedsUpdate] = React.useState(true);
	const [page, setPage] = React.useState(0);
	const submit = useSubmit();
	const updateAction = useFormAction('update');

	const {enqueueSnackbar} = useSnackbar();

	const handleChangePage = (
		event: React.MouseEvent<HTMLButtonElement> | null,
		newPage: number,
	) => {
		setPage(newPage);
	};

	React.useEffect(() => {
		if (needsUpdate) {
			getExpenses(props.investmentId)
				.then((data) => {
					setExpenses(data);
					setNeedsUpdate(false);
				})
				.catch((error) => {
					enqueueSnackbar(`Failed to get expenses: ${error.message}`, {variant: 'error'});
				});
		}
	}, [enqueueSnackbar, needsUpdate, props.investmentId, submit, updateAction]);

	const handleDelete = (expenseId: number) => {
		deleteExpense(expenseId)
			.then(() => {
				enqueueSnackbar('Expense deleted successfully.', {variant: 'success'});
				setNeedsUpdate(true);
			})
			.catch((error) => {
				enqueueSnackbar(`Failed to delete expense:${error.message}`, {variant: 'error'});
			});
	};

	return (
		<Box sx={{width: '100%', alignSelf: 'start'}}>
			<Button
				sx={{margin: '0.25rem'}}
				startIcon={<Icon baseClassName='material-icons'>add</Icon>}
				onClick={() => setAddDialogOpen(true)}>
				Add Expense
			</Button>
			<TableContainer component={Paper} sx={{minWidth: '100%'}}>
				<Table sx={{minWidth: '100%'}} size='small'>
					<TableHead>
						<TableRow>
							<TableCell>Amount</TableCell>
							<TableCell>Expense Type</TableCell>
							<TableCell>Expense Date(s)</TableCell>
							<TableCell>Recurrence</TableCell>
							<TableCell></TableCell>
						</TableRow>
					</TableHead>
					<TableBody>
						{expenses.map((expense) => (
							<TableRow key={expense.expenseId}>
								<TableCell>${expense.amount.toFixed(2)}</TableCell>
								<TableCell>{expense.expenseType}</TableCell>
								<TableCell>
									{expense.expenseType === 'Recurring'
										? `${expense.expenseData['startDate']} - ${expense.expenseData['endDate']}`
										: expense.expenseData['date']}
								</TableCell>
								<TableCell>
									{expense.expenseType === 'OneTime'
										? 'N/A'
										: `${expense.expenseData['frequency']} mos.`}
								</TableCell>
								<TableCell padding='checkbox'>
									<IconButton
										onClick={() => handleDelete(expense.expenseId)}
										size='small'>
										<Icon baseClassName='material-icons'>delete</Icon>
									</IconButton>
								</TableCell>
							</TableRow>
						))}
					</TableBody>
					<TableFooter>
						<TablePagination
							count={expenses.length}
							rowsPerPage={10}
							page={page}
							onPageChange={handleChangePage}
							rowsPerPageOptions={[]}
							ActionsComponent={TablePaginationActions}
						/>
					</TableFooter>
				</Table>
			</TableContainer>
			<AddExpenseDialog
				show={addDialogOpen}
				onClose={() => setAddDialogOpen(false)}
				investmentId={props.investmentId}
				setNeedsUpdate={setNeedsUpdate}
			/>
		</Box>
	);
};
