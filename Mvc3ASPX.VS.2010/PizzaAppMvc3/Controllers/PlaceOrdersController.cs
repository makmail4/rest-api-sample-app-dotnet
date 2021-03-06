﻿using PayPal;
using PayPal.Api.Payments;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace PizzaAppMvc3
{
    public class PlaceOrdersController : Controller
    {
        #region Data
        private DataAccessLayer dataAccessObject;

        private bool Update(int orderID, string pymntID, string state, string amount, string description, string updatedAt)
        {
            bool isSuccess = false;
            int rowsAffacted = 0;
            StringBuilder sqliteQueryUpdate = new StringBuilder();
            sqliteQueryUpdate.Append("UPDATE orders ");
            sqliteQueryUpdate.Append("SET ");
            sqliteQueryUpdate.Append("payment_id = @payment_id, ");
            sqliteQueryUpdate.Append("state = @state, ");
            sqliteQueryUpdate.Append("amount = @amount, ");
            sqliteQueryUpdate.Append("description = @description, ");
            sqliteQueryUpdate.Append("updated_at = @updated_at ");
            sqliteQueryUpdate.Append("WHERE ");
            sqliteQueryUpdate.Append("id = @id");
            SQLiteDataAdapter sqliteDataAdapterUpdate = new SQLiteDataAdapter();
            sqliteDataAdapterUpdate.UpdateCommand = new SQLiteCommand();
            sqliteDataAdapterUpdate.UpdateCommand.Parameters.AddWithValue("@payment_id", pymntID);
            sqliteDataAdapterUpdate.UpdateCommand.Parameters.AddWithValue("@state", state);
            sqliteDataAdapterUpdate.UpdateCommand.Parameters.AddWithValue("@amount", amount);
            sqliteDataAdapterUpdate.UpdateCommand.Parameters.AddWithValue("@description", description);
            sqliteDataAdapterUpdate.UpdateCommand.Parameters.AddWithValue("@updated_at", updatedAt);
            sqliteDataAdapterUpdate.UpdateCommand.Parameters.AddWithValue("@id", orderID);
            dataAccessObject = new DataAccessLayer();
            rowsAffacted = dataAccessObject.Update(sqliteQueryUpdate.ToString(), sqliteDataAdapterUpdate);
            if (rowsAffacted > 0)
            {
                isSuccess = true;
            }
            return isSuccess;
        }

        private bool Insert(int userID, string pymntID, string state, string amount, string description, string createdAt, string updatedAt)
        {
            bool isSuccess = false;
            int rowsAffacted = 0;
            StringBuilder sqliteQueryInsert = new StringBuilder();
            sqliteQueryInsert.Append("INSERT INTO orders");
            sqliteQueryInsert.Append("(");
            sqliteQueryInsert.Append("user_id, ");
            sqliteQueryInsert.Append("payment_id, ");
            sqliteQueryInsert.Append("state, ");
            sqliteQueryInsert.Append("amount, ");
            sqliteQueryInsert.Append("description, ");
            sqliteQueryInsert.Append("created_at, ");
            sqliteQueryInsert.Append("updated_at ");
            sqliteQueryInsert.Append(") ");
            sqliteQueryInsert.Append("VALUES ");
            sqliteQueryInsert.Append("(");
            sqliteQueryInsert.Append("@user_id, ");
            sqliteQueryInsert.Append("@payment_id, ");
            sqliteQueryInsert.Append("@state, ");
            sqliteQueryInsert.Append("@amount,");
            sqliteQueryInsert.Append("@description, ");
            sqliteQueryInsert.Append("@created_at, ");
            sqliteQueryInsert.Append("@updated_at ");
            sqliteQueryInsert.Append(")");
            SQLiteDataAdapter sqliteDataAdapterInsert = new SQLiteDataAdapter();
            sqliteDataAdapterInsert.InsertCommand = new SQLiteCommand();
            sqliteDataAdapterInsert.InsertCommand.Parameters.AddWithValue("@user_id", userID);
            sqliteDataAdapterInsert.InsertCommand.Parameters.AddWithValue("@payment_id", pymntID);
            sqliteDataAdapterInsert.InsertCommand.Parameters.AddWithValue("@state", state);
            sqliteDataAdapterInsert.InsertCommand.Parameters.AddWithValue("@amount", amount);
            sqliteDataAdapterInsert.InsertCommand.Parameters.AddWithValue("@description", description);
            sqliteDataAdapterInsert.InsertCommand.Parameters.AddWithValue("@created_at", createdAt);
            sqliteDataAdapterInsert.InsertCommand.Parameters.AddWithValue("@updated_at", updatedAt);
            dataAccessObject = new DataAccessLayer();
            rowsAffacted = dataAccessObject.Insert(sqliteQueryInsert.ToString(), sqliteDataAdapterInsert);
            if (rowsAffacted > 0)
            {
                isSuccess = true;
            }
            return isSuccess;
        }

        private bool Insert(int userID, string createdAt, string updatedAt)
        {
            bool isSuccess = false;
            int rowsAffacted = 0;
            StringBuilder sqliteQueryInsert = new StringBuilder();
            sqliteQueryInsert.Append("INSERT INTO orders");
            sqliteQueryInsert.Append("(");
            sqliteQueryInsert.Append("user_id, ");
            sqliteQueryInsert.Append("created_at, ");
            sqliteQueryInsert.Append("updated_at ");
            sqliteQueryInsert.Append(") ");
            sqliteQueryInsert.Append("VALUES ");
            sqliteQueryInsert.Append("(");
            sqliteQueryInsert.Append("@user_id, ");
            sqliteQueryInsert.Append("@created_at, ");
            sqliteQueryInsert.Append("@updated_at ");
            sqliteQueryInsert.Append(")");
            SQLiteDataAdapter sqliteDataAdapterInsert = new SQLiteDataAdapter();
            sqliteDataAdapterInsert.InsertCommand = new SQLiteCommand();
            sqliteDataAdapterInsert.InsertCommand.Parameters.AddWithValue("@user_id", userID);
            sqliteDataAdapterInsert.InsertCommand.Parameters.AddWithValue("@created_at", createdAt);
            sqliteDataAdapterInsert.InsertCommand.Parameters.AddWithValue("@updated_at", updatedAt);
            dataAccessObject = new DataAccessLayer();
            rowsAffacted = dataAccessObject.Insert(sqliteQueryInsert.ToString(), sqliteDataAdapterInsert);
            if (rowsAffacted > 0)
            {
                isSuccess = true;
            }
            return isSuccess;
        }

        private DataTable GetUser(string email)
        {
            DataTable datTable = new DataTable();
            StringBuilder sqliteQuerySelect = new StringBuilder();
            sqliteQuerySelect.Append("SELECT ");
            sqliteQuerySelect.Append("id, ");
            sqliteQuerySelect.Append("email, ");
            sqliteQuerySelect.Append("encrypted_password, ");
            sqliteQuerySelect.Append("sign_in_count, ");
            sqliteQuerySelect.Append("credit_card_id ");
            sqliteQuerySelect.Append("FROM users ");
            sqliteQuerySelect.Append("WHERE email = @email");
            SQLiteDataAdapter sqliteDataAdapterSelect = new SQLiteDataAdapter();
            sqliteDataAdapterSelect.SelectCommand = new SQLiteCommand();
            sqliteDataAdapterSelect.SelectCommand.Parameters.AddWithValue("@email", email);
            dataAccessObject = new DataAccessLayer();
            datTable = dataAccessObject.Select(sqliteQuerySelect.ToString(), sqliteDataAdapterSelect);
            return datTable;
        }

        private DataTable GetOrders(int userID)
        {
            DataTable datTable = new DataTable();
            StringBuilder sqliteQuerySelect = new StringBuilder();
            sqliteQuerySelect.Append("SELECT ");
            sqliteQuerySelect.Append("DISTINCT id, ");
            sqliteQuerySelect.Append("user_id, ");
            sqliteQuerySelect.Append("payment_id, ");
            sqliteQuerySelect.Append("state, ");
            sqliteQuerySelect.Append("amount, ");
            sqliteQuerySelect.Append("description, ");
            sqliteQuerySelect.Append("created_at, ");
            sqliteQuerySelect.Append("updated_at ");
            sqliteQuerySelect.Append("FROM orders ");
            sqliteQuerySelect.Append("WHERE user_id = @user_id ORDER BY id DESC LIMIT 1");
            SQLiteDataAdapter sqliteDataAdapterSelect = new SQLiteDataAdapter();
            sqliteDataAdapterSelect.SelectCommand = new SQLiteCommand();
            sqliteDataAdapterSelect.SelectCommand.Parameters.AddWithValue("@user_id", userID);
            dataAccessObject = new DataAccessLayer();
            datTable = dataAccessObject.Select(sqliteQuerySelect.ToString(), sqliteDataAdapterSelect);
            return datTable;
        }

        private int GetSignedInUserID(string email)
        {
            int userID = 0;
            DataTable datTable = GetUser(email);
            if (datTable != null && datTable.Rows.Count > 0)
            {
                var distinctRows = from DataRow dRow in datTable.Rows
                                   where dRow.Field<string>("email") == email
                                   select new { column1 = dRow["id"] };
                if (distinctRows != null)
                {
                    foreach (var row in distinctRows)
                    {
                        userID = Convert.ToInt32(row.column1);
                        break;
                    }
                }
            }
            return userID;
        }

        private string GetSignedInUserCreditCardID(string email)
        {
            string creditCardID = string.Empty;

            CreditCard crdtCard = new CreditCard();
            DataTable datTable = GetUser(email);
            if (datTable != null && datTable.Rows.Count > 0)
            {
                var distinctRows = from DataRow dRow in datTable.Rows
                                   where dRow.Field<string>("email") == email
                                   select new { column1 = dRow["credit_card_id"] };
                if (distinctRows != null)
                {
                    foreach (var row in distinctRows)
                    {
                        creditCardID = Convert.ToString(row.column1);
                        break;
                    }
                }
            }
            return creditCardID;
        }

        private int GetSignedInUserLastInsertedOrderID(int userID)
        {
            int orderID = 0;
            CreditCard crdtCard = new CreditCard();
            DataTable datTable = GetOrders(userID);
            if (datTable != null && datTable.Rows.Count > 0)
            {
                var distinctRows = from DataRow dRow in datTable.Rows
                                   select new { column1 = dRow["id"] };
                if (distinctRows != null)
                {
                    foreach (var row in distinctRows)
                    {
                        orderID = Convert.ToInt32(row.column1);
                        break;
                    }
                }
            }
            return orderID;
        }
        #endregion

        #region PayPal
        private string AccessToken
        {
            get
            {
                string token = new OAuthTokenCredential
                                (
                                   "EBWKjlELKMYqRNQ6sYvFo64FtaRLRR5BdHEESmha49TM",
                                    "EO422dn3gQLgDbuwqTjzrFgFtaRLRR5BdHEESmha49TM",
                                    Configuration.GetConfiguration()
                                ).GetAccessToken();
                return token;
            }
        }

        private APIContext Api
        {
            get
            {
                APIContext context = new APIContext(AccessToken);
                context.Config = Configuration.GetConfiguration();
                return context;
            }
        }

        public Payment CreatePayment(string email, PaymentMethod paymntMethod, string orderAmount, string orderDescription, string returnUrl, string cancelUrl)
        {
            Payment pymnt = null;

            Details amountDetails = new Details();
            amountDetails.shipping = "2";
            amountDetails.tax = "1";
            amountDetails.subtotal = orderAmount;

            Amount amount = new Amount();
            amount.currency = "USD";
            int total = Convert.ToInt32(amountDetails.tax) + Convert.ToInt32(amountDetails.shipping) + Convert.ToInt32(orderAmount);
            amount.total = total.ToString();
            amount.details = amountDetails;

            RedirectUrls redirectUrls = new RedirectUrls();
            redirectUrls.return_url = returnUrl;
            redirectUrls.cancel_url = cancelUrl;

            Transaction transaction = new Transaction();
            transaction.amount = amount;
            transaction.description = orderDescription;
            List<Transaction> transactions = new List<Transaction>();
            transactions.Add(transaction);

            Payer payer = new Payer();
            payer.payment_method = paymntMethod.ToString();

            Payment pyment = new Payment();
            pyment.intent = "sale";
            pyment.payer = payer;
            pyment.transactions = transactions;
            pyment.redirect_urls = redirectUrls;

            pymnt = pyment.Create(Api);
            return pymnt;
        }

        public Payment CreatePayment(string email, PaymentMethod paymntMethod, string orderAmount, string orderDescription)
        {
            Payment pay = null;

            Amount amount = new Amount();
            amount.currency = "USD";
            amount.total = orderAmount;

            Transaction transaction = new Transaction();
            transaction.amount = amount;
            transaction.description = orderDescription;

            List<Transaction> transactions = new List<Transaction>();
            transactions.Add(transaction);

            FundingInstrument fundingInstrument = new FundingInstrument();
            CreditCardToken creditCardToken = new CreditCardToken();
            creditCardToken.credit_card_id = GetSignedInUserCreditCardID(email);
            fundingInstrument.credit_card_token = creditCardToken;

            List<FundingInstrument> fundingInstrumentList = new List<FundingInstrument>();
            fundingInstrumentList.Add(fundingInstrument);

            Payer payer = new Payer();
            payer.funding_instruments = fundingInstrumentList;
            payer.payment_method = paymntMethod.ToString();

            Payment pyment = new Payment();
            pyment.intent = "sale";
            pyment.payer = payer;
            pyment.transactions = transactions;
            pay = pyment.Create(Api);
            return pay;
        }

        private string GetApprovalURL(Payment payment)
        {
            string redirectUrl = null;
            List<Links> links = payment.links;
            foreach (Links lnk in links)
            {
                if (lnk.rel.ToLower().Equals("approval_url"))
                {
                    redirectUrl = Server.UrlDecode(lnk.href);
                    break;
                }
            }
            return redirectUrl;
        }
        #endregion

        #region Register
        private SelectListItem[] RegisterPaymentTypes()
        {
            var model = new PlaceOrdersModels();
            model.PaymentTypes = new[]
            {
                new SelectListItem { Text = "credit_card", Value = "credit_card" }, 
                new SelectListItem { Text = "paypal", Value = "paypal" },
            };
            return model.PaymentTypes;
        }
        #endregion

        #region ActionResult

        //
        // GET: /PlaceOrders/

        [Authorize]
        public ActionResult PlaceOrders()
        {
            var model = new PlaceOrdersModels();
            model.PaymentTypes = RegisterPaymentTypes();
            model.PaymentType = string.Empty;

            if (Request.QueryString["order[amount]"] != null && Request.QueryString["order[description]"] != null)
            {
                model.Amount = Request.QueryString["order[amount]"];
                model.Description = Request.QueryString["order[description]"];
            }

            return View(model);
        }

        //
        // POST: /PlaceOrders/

        [Authorize]
        [HttpPost]
        public ActionResult PlaceOrders(PlaceOrdersModels model)
        {
            if (ModelState.IsValid)
            {
                if (Request.QueryString["order[amount]"] != null && Request.QueryString["order[description]"] != null)
                {
                    model.Amount = Request.QueryString["order[amount]"];
                    model.Description = Request.QueryString["order[description]"];

                    var email = User.Identity.Name.Trim();
                    var userID = GetSignedInUserID(email);

                    if (model.PaymentType != null)
                    {
                        if (model.PaymentType.Trim() == "credit_card")
                        {
                            var amount = model.Amount.Trim();
                            var description = model.Description.Trim();
                            Payment pymnt = null;
                            pymnt = CreatePayment(email, PaymentMethod.credit_card, amount, description);
                            if (pymnt != null)
                            {
                                var pymntID = pymnt.id;
                                var state = pymnt.state;
                                DateTime createdDateTime = Convert.ToDateTime(pymnt.create_time);
                                var createdAt = createdDateTime.ToString("yyyy-MM-dd hh:mm:ss.FFFFF");
                                var updatedAt = createdDateTime.ToString("yyyy-MM-dd hh:mm:ss.FFFFF");
                                bool isSuccess = Insert(userID, pymntID, state, amount, description, createdAt, updatedAt);
                                if (isSuccess)
                                {
                                    if (state.Trim().ToLower().Equals("approved"))
                                    {
                                        string requestUrl = Request.Url.OriginalString;
                                        string authority = Request.Url.Authority;
                                        string dnsSafeHost = Request.Url.DnsSafeHost;

                                        if (Request.UrlReferrer != null && Request.UrlReferrer.Scheme == "https")
                                        {
                                            requestUrl = requestUrl.Replace("http://", "https://");
                                            requestUrl = requestUrl.Replace(authority, dnsSafeHost);
                                        }
                                        return new RedirectResult("~/Orders/Orders?Status=approved");
                                    }
                                    else
                                    {
                                        ModelState.AddModelError(string.Empty, "Order failed.");
                                    }
                                }
                            }
                            else
                            {
                                ModelState.AddModelError(string.Empty, "Order failed.");
                            }
                        }
                        else if (model.PaymentType.Trim() == "paypal")
                        {
                            DateTime createdDateTime = DateTime.Now;
                            var createdAt = createdDateTime.ToString("yyyy-MM-dd hh:mm:ss.FFFFF");
                            var updatedAt = createdDateTime.ToString("yyyy-MM-dd hh:mm:ss.FFFFF");
                            bool isSuccess = Insert(userID, createdAt, updatedAt);
                            if (isSuccess)
                            {
                                int orderID = GetSignedInUserLastInsertedOrderID(userID);
                                string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/Orders/Orders?";
                                string requestUrl = Request.Url.OriginalString;
                                string returnUrl = baseURI + "Success=True&OrderID=" + orderID;
                                string cancelUrl = baseURI + "Success=False&OrderID=" + orderID;
                                var amount = model.Amount.Trim();
                                var description = model.Description.Trim();
                                Payment pymnt = null;

                                pymnt = CreatePayment(email, PaymentMethod.paypal, amount, description, returnUrl, cancelUrl);
                                if (pymnt != null)
                                {
                                    var pymntID = pymnt.id;
                                    var state = pymnt.state;
                                    var updatedAtDateTime = Convert.ToDateTime(pymnt.create_time);
                                    var pymntUpdatedAt = updatedAtDateTime.ToString("yyyy-MM-dd hh:mm:ss.FFFFF");
                                    bool isUpdateSuccess = Update(orderID, pymntID, state, amount, description, pymntUpdatedAt);
                                    if (isUpdateSuccess)
                                    {
                                        string dredirectUrl = GetApprovalURL(pymnt);
                                        return new RedirectResult(dredirectUrl);
                                    }
                                }
                                else
                                {
                                    ModelState.AddModelError(string.Empty, "Order failed.");
                                }
                            }
                        }
                    }
                }

            }

            if (model.PaymentTypes == null)
            {
                model.PaymentTypes = RegisterPaymentTypes();
            }
            return View(model);
        }

        #endregion
    }
}
