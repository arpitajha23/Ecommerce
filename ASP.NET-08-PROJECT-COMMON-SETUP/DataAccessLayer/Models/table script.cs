using Microsoft.AspNetCore.Http.HttpResults;
using PresentationLayer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DataAccessLayer.Models
{
    internal class table_script
    {
        //        -- select* from appusers

        //-- CREATE TABLE categories(
        //--     category_id SERIAL PRIMARY KEY,
        //--     category_name VARCHAR(255) NOT NULL,
        //--     description TEXT,
        //--     parent_category_id INT,
        //--     FOREIGN KEY(parent_category_id) REFERENCES categories(category_id) ON DELETE SET NULL
        //-- );

        //-- CREATE TABLE products(
        //--     product_id SERIAL PRIMARY KEY,
        //--     name VARCHAR(255),
        //--     description TEXT,
        //--     price NUMERIC(10,2),
        //--     stock_quantity INT,
        //--     category_id INT,
        //--     is_active BOOLEAN DEFAULT TRUE,
        //--     created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
        //--     FOREIGN KEY(category_id) REFERENCES categories(category_id)
        //-- );


        //-- CREATE TABLE product_images(
        //--     image_id SERIAL PRIMARY KEY,
        //--     product_id INT,
        //--     image_url TEXT,
        //--     is_primary BOOLEAN DEFAULT FALSE,
        //--     FOREIGN KEY (product_id) REFERENCES products(product_id) ON DELETE CASCADE
        //-- );

        //        CREATE TABLE orders(
        //            order_id SERIAL PRIMARY KEY,
        //            user_id INT,
        //            order_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
        //            status VARCHAR(50),
        //    total_amount NUMERIC(10,2),
        //    shipping_address TEXT,
        //    payment_status VARCHAR(50),
        //    FOREIGN KEY(user_id) REFERENCES appusers(id)
        //);

        //CREATE TABLE order_items(
        //    order_item_id SERIAL PRIMARY KEY,
        //    order_id INT,
        //    product_id INT,
        //    quantity INT,
        //    unit_price NUMERIC(10,2),
        //    FOREIGN KEY(order_id) REFERENCES orders(order_id),
        //    FOREIGN KEY(product_id) REFERENCES products(product_id)
        //);

        //CREATE TABLE carts(
        //        cart_id SERIAL PRIMARY KEY,
        //        user_id INT,
        //    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
        //    FOREIGN KEY (user_id) REFERENCES appusers(id)
        //);

        //        CREATE TABLE cart_items(
        //            cart_item_id SERIAL PRIMARY KEY,
        //            cart_id INT,
        //            product_id INT,
        //            quantity INT,
        //            FOREIGN KEY (cart_id) REFERENCES carts(cart_id) ON DELETE CASCADE,
        //            FOREIGN KEY (product_id) REFERENCES products(product_id)
        //        );

        //        CREATE TABLE wishlists(
        //            wishlist_id SERIAL PRIMARY KEY,
        //            user_id INT,
        //        product_id INT,
        //            created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
        //            FOREIGN KEY (user_id) REFERENCES appusers(id),
        //            FOREIGN KEY (product_id) REFERENCES products(product_id)
        //        );

        //        CREATE TABLE product_reviews(
        //            review_id SERIAL PRIMARY KEY,
        //            product_id INT,
        //            user_id INT,
        //            rating INT CHECK (rating >= 1 AND rating <= 5),
        //    comment TEXT,
        //    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
        //    FOREIGN KEY(product_id) REFERENCES products(product_id),
        //    FOREIGN KEY(user_id) REFERENCES appusers(id)
        //);


        //CREATE TABLE payments(
        //    payment_id SERIAL PRIMARY KEY,
        //    order_id INT,
        //    payment_method VARCHAR(50),
        //    payment_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
        //    payment_status VARCHAR(50),
        //    transaction_id VARCHAR(100),
        //    FOREIGN KEY(order_id) REFERENCES orders(order_id)
        //);

//        ALTER TABLE products
//ADD COLUMN seller_id INT;

//ALTER TABLE products
//ADD CONSTRAINT fk_seller
//FOREIGN KEY(seller_id) REFERENCES seller_users(seller_id);

//        CREATE TABLE seller_users(
//            seller_id SERIAL PRIMARY KEY,
//            seller_name VARCHAR(100),
//    email VARCHAR(255) UNIQUE NOT NULL,
//    phone_number VARCHAR(20),
//    address TEXT,
//    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
//    is_active BOOLEAN DEFAULT TRUE
//);


    }
}
