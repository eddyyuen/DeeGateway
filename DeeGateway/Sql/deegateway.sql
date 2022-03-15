/*
 Navicat Premium Data Transfer

 Source Server         : 127.0.0.1
 Source Server Type    : MySQL
 Source Server Version : 80018
 Source Host           : localhost:3306
 Source Schema         : deegateway

 Target Server Type    : MySQL
 Target Server Version : 80018
 File Encoding         : 65001

 Date: 25/10/2019 12:43:48
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for header
-- ----------------------------
DROP TABLE IF EXISTS `header`;
CREATE TABLE `header`  (
  `id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `title` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `urlroute_id` int(10) UNSIGNED NOT NULL,
  `condition_type` int(10) NOT NULL,
  `condition_value` text CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `extractor` text CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  `handle` text CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `skip_next` int(1) UNSIGNED NOT NULL DEFAULT 0,
  `order` int(10) UNSIGNED NOT NULL DEFAULT 1000,
  `enable` int(10) NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 7 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of header
-- ----------------------------
INSERT INTO `header` VALUES (6, 'api.scigeeker.com', 6, 1, '[{\"order\":1,\"key\":\"\",\"type\":1,\"value\":\".*\",\"handle\":1,\"default_value\":null}]', '[]', '[{\"key\":\"Host\",\"value\":\"api.scigeeker.com\",\"default_value\":\"\",\"LAY_TABLE_INDEX\":0}]', 0, 1000, 1);

-- ----------------------------
-- Table structure for jwt_auth
-- ----------------------------
DROP TABLE IF EXISTS `jwt_auth`;
CREATE TABLE `jwt_auth`  (
  `id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `title` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `urlroute_id` int(10) UNSIGNED NOT NULL,
  `condition_type` int(10) NOT NULL,
  `condition_value` text CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `extractor` text CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  `handle` text CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `skip_next` int(1) UNSIGNED NOT NULL DEFAULT 0,
  `order` int(10) UNSIGNED NOT NULL DEFAULT 1000,
  `enable` int(10) NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 13 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of jwt_auth
-- ----------------------------
INSERT INTO `jwt_auth` VALUES (2, '2', 2, 1, '[{\"order\":1,\"key\":\"\",\"type\":1,\"value\":\".*\",\"handle\":1,\"default_value\":null}]', '[{\"order\":1,\"key\":\"\",\"type\":1,\"value\":\"/api/1\",\"handle\":1,\"default_value\":null}]', '{\"status\":\"42951\",\"message\":\"1\",\"returncode\":\"25\"}', 1, 1000, 1);
INSERT INTO `jwt_auth` VALUES (12, '默认', 6, 1, '[{\"order\":1,\"key\":\"\",\"type\":1,\"value\":\".*\",\"handle\":1,\"default_value\":null}]', '[{\"order\":1,\"key\":\"\",\"type\":1,\"value\":\"^/api/.*\",\"handle\":1,\"default_value\":null}]', '{\"status\":\"495\",\"message\":\"auth\",\"returncode\":\"2\"}', 0, 1000, 1);

-- ----------------------------
-- Table structure for plugins
-- ----------------------------
DROP TABLE IF EXISTS `plugins`;
CREATE TABLE `plugins`  (
  `id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `plugin_name` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT 'plugin name',
  `enable` int(255) UNSIGNED NOT NULL DEFAULT 0 COMMENT '0-disable 1-enable',
  `type` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `level` int(2) UNSIGNED NOT NULL DEFAULT 1000,
  `setting` json NOT NULL,
  `description` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '描述',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 6 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of plugins
-- ----------------------------
INSERT INTO `plugins` VALUES (1, 'Requesting_JwtAuth', 1, 'Requesting', 1, '{\"token\": \"12qyg4coej88uqromo0xdmx4y0il5dn5y7b72tlb3imba677ht1p1xlfcnh36mk5u3xzjktfara29axvzk85apfplun7oslbe1m20c148p5d519kja5wvg7lmn5v4a5ou\", \"timeout\": 1440}', 'jwt权限验证111');
INSERT INTO `plugins` VALUES (2, 'AgentRequesting_Header', 1, 'AgentRequesting', 1, 'null', 'request header处理');
INSERT INTO `plugins` VALUES (3, 'AgentRequesting_Rewrite', 1, 'AgentRequesting', 1, 'null', 'url重写');
INSERT INTO `plugins` VALUES (5, 'Requesting_UrlVerify', 0, 'Requesting', 1, '{}', 'url验证是否符合路由');

-- ----------------------------
-- Table structure for ratelimit
-- ----------------------------
DROP TABLE IF EXISTS `ratelimit`;
CREATE TABLE `ratelimit`  (
  `id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `urlroute_id` int(10) NOT NULL,
  `rule_id` int(10) NOT NULL,
  `enable` int(10) UNSIGNED NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 2 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of ratelimit
-- ----------------------------
INSERT INTO `ratelimit` VALUES (1, 1, 1, 1);

-- ----------------------------
-- Table structure for rewrite
-- ----------------------------
DROP TABLE IF EXISTS `rewrite`;
CREATE TABLE `rewrite`  (
  `id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `title` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `urlroute_id` int(10) UNSIGNED NOT NULL,
  `condition_type` int(10) NOT NULL,
  `condition_value` text CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `extractor` text CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  `handle` text CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `skip_next` int(1) UNSIGNED NOT NULL DEFAULT 0,
  `order` int(10) UNSIGNED NOT NULL DEFAULT 1000,
  `enable` int(10) NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 4 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of rewrite
-- ----------------------------
INSERT INTO `rewrite` VALUES (1, 'Rewrte', 1, 1, '[{\"order\":1,\"key\":null,\"type\":2,\"value\":\"^/status.*\",\"handle\":2,\"default_value\":null}]', '[{\"order\":1,\"key\":\"Host\",\"type\":1,\"value\":\"^/status/(.*)\",\"handle\":0,\"default_value\":null}]', '{\"url\":\"{0}\"}', 1, 1000, 1);
INSERT INTO `rewrite` VALUES (3, '测试rewrite', 6, 1, '[{\"order\":1,\"key\":\"\",\"type\":1,\"value\":\".*\",\"handle\":1,\"default_value\":null}]', '[{\"order\":1,\"key\":\"\",\"type\":1,\"value\":\"/api/(.*)\",\"handle\":1,\"default_value\":\"\"}]', '{\"url\":\"/{0}\"}', 0, 1000, 1);

-- ----------------------------
-- Table structure for server
-- ----------------------------
DROP TABLE IF EXISTS `server`;
CREATE TABLE `server`  (
  `id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `server_name` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '服务名称',
  `uri` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '服务地址 http://127.0.0.1:80/',
  `max_connections` int(10) UNSIGNED NOT NULL DEFAULT 1000 COMMENT '最大连接300',
  `enable` int(1) UNSIGNED NOT NULL DEFAULT 0 COMMENT '是否启用',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 3 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of server
-- ----------------------------
INSERT INTO `server` VALUES (1, 'Demo Api接口-10', 'http://127.0.0.1:9050/', 200, 0);
INSERT INTO `server` VALUES (2, 'Demo Api接口-2', 'http://127.0.0.1:9091/', 300, 1);

-- ----------------------------
-- Table structure for urlroute
-- ----------------------------
DROP TABLE IF EXISTS `urlroute`;
CREATE TABLE `urlroute`  (
  `id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `urlroute_name` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `url` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `hash_pattern` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `enable` int(255) UNSIGNED NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 7 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of urlroute
-- ----------------------------
INSERT INTO `urlroute` VALUES (1, '服务器状态接口', '^/status.*', '', 1);
INSERT INTO `urlroute` VALUES (2, 'API网关', '^/api/.*', '', 1);
INSERT INTO `urlroute` VALUES (6, '默认页面', '.*', '', 1);

-- ----------------------------
-- Table structure for urlroute_server
-- ----------------------------
DROP TABLE IF EXISTS `urlroute_server`;
CREATE TABLE `urlroute_server`  (
  `id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `urlroute_id` int(11) NOT NULL,
  `server_id` int(255) NOT NULL,
  `weight` int(10) UNSIGNED NOT NULL DEFAULT 10,
  `maxrps` int(255) NOT NULL DEFAULT 0,
  `enable` int(255) UNSIGNED NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 10 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of urlroute_server
-- ----------------------------
INSERT INTO `urlroute_server` VALUES (1, 1, 2, 10, 0, 0);
INSERT INTO `urlroute_server` VALUES (4, 2, 1, 10, 0, 0);
INSERT INTO `urlroute_server` VALUES (5, 2, 2, 10, 0, 0);
INSERT INTO `urlroute_server` VALUES (7, 1, 1, 10, 0, 1);
INSERT INTO `urlroute_server` VALUES (9, 6, 1, 10, 0, 1);

SET FOREIGN_KEY_CHECKS = 1;
