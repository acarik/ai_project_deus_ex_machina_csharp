maxS = 50;

load data01.txt
load data02.txt
load data03.txt
load data04.txt
load data05.txt
load data06.txt
load data07.txt

data01(maxS,2) = 0;
data02(maxS,2) = 0;
data03(maxS,2) = 0;
data04(maxS,2) = 0;
data05(maxS,2) = 0;
data06(maxS,2) = 0;
data07(maxS,2) = 0;

for i = 1:7
    eval(['data(:,:,i) = data0' num2str(i) ';'])
end

% 1: m4 a1 h0
% 2: m4 a0 h0
% 3: m6 a1 h0
% 4: m6 a0 h0
% 5: m6 a0 h1
% 6: m4 a1 h1
% 7: m4 a0 h1
% 11: m4 a1 h0
i = 11;
m = load('ai_sonuc\alpha_var_hash_yok_1_pat_3\data01.txt');
m(maxS,2) = 0;
data(:,:,i) = m;
% 12: m4 a1 h0
i = i + 1;
m = load('ai_sonuc\alpha_var_hash_yok_1_pat_3\data02.txt');
m(maxS,2) = 0;
data(:,:,i) = m;
% 13: m4 a1 h0
i = i + 1;
m = load('ai_sonuc\alpha_var_hash_yok_1_pat_3\data03.txt');
m(maxS,2) = 0;
data(:,:,i) = m;
% 14: m4 a1 h0
i = i + 1;
m = load('ai_sonuc\alpha_var_hash_yok_1_pat_3\data04.txt');
m(maxS,2) = 0;
data(:,:,i) = m;
% 15: m4 a1 h0
i = i + 1;
m = load('ai_sonuc\alpha_var_hash_yok_1_pat_3\data05.txt');
m(maxS,2) = 0;
data(:,:,i) = m;
% 16: m4 a1 h0
i = i + 1;
m = load('ai_sonuc\alpha_var_hash_yok_1_pat_3\data06.txt');
m(maxS,2) = 0;
data(:,:,i) = m;
% 17: m4 a1 h0
i = i + 1;
m = load('ai_sonuc\alpha_var_hash_yok_1_pat_3\data07.txt');
m(maxS,2) = 0;
data(:,:,i) = m;
% 18: m4 a1 h0
i = i + 1;
m = load('ai_sonuc\alpha_var_hash_yok_1_pat_3\data08.txt');
m(maxS,2) = 0;
data(:,:,i) = m;
% 19: m4 a1 h0
i = i + 1;
m = load('ai_sonuc\alpha_var_hash_yok_1_pat_3\data09.txt');
m(maxS,2) = 0;
data(:,:,i) = m;
% 20: m4 a1 h0
i = i + 1;
m = load('ai_sonuc\alpha_var_hash_yok_1_pat_3\data10.txt');
m(maxS,2) = 0;
data(:,:,i) = m;
% 21: m4 a0 h0
i = i + 1;
m = load('ai_sonuc\alpha_yok_hash_yok_3\data01.txt');
m(maxS,2) = 0;
data(:,:,i) = m;
% 22: m4 a0 h0
i = i + 1;
m = load('ai_sonuc\alpha_yok_hash_yok_3\data02.txt');
m(maxS,2) = 0;
data(:,:,i) = m;
% 23: m4 a0 h0
i = i + 1;
m = load('ai_sonuc\alpha_yok_hash_yok_3\data03.txt');
m(maxS,2) = 0;
data(:,:,i) = m;
% 24: m4 a0 h0
i = i + 1;
m = load('ai_sonuc\alpha_yok_hash_yok_3\data04.txt');
m(maxS,2) = 0;
data(:,:,i) = m;
% 25: m4 a0 h0
i = i + 1;
m = load('ai_sonuc\alpha_yok_hash_yok_3\data05.txt');
m(maxS,2) = 0;
data(:,:,i) = m;
% 26: m4 a0 h0
i = i + 1;
m = load('ai_sonuc\alpha_yok_hash_yok_3\data06.txt');
m(maxS,2) = 0;
data(:,:,i) = m;
% 27: m4 a0 h0
i = i + 1;
m = load('ai_sonuc\alpha_yok_hash_yok_3\data07.txt');
m(maxS,2) = 0;
data(:,:,i) = m;
% 28: m4 a0 h0
i = i + 1;
m = load('ai_sonuc\alpha_yok_hash_yok_3\data08.txt');
m(maxS,2) = 0;
data(:,:,i) = m;
% 29: m4 a0 h0
i = i + 1;
m = load('ai_sonuc\alpha_yok_hash_yok_3\data09.txt');
m(maxS,2) = 0;
data(:,:,i) = m;
% 30: m4 a0 h0
i = i + 1;
m = load('ai_sonuc\alpha_yok_hash_yok_3\data10.txt');
m(maxS,2) = 0;
data(:,:,i) = m;
% 31: m4 a0 h1
i = i + 1;
m = load('ai_sonuc\alpha_yok_hash_var_4\data01.txt');
m(maxS,2) = 0;
data(:,:,i) = m;
% 32: m4 a0 h1
i = i + 1;
m = load('ai_sonuc\alpha_yok_hash_var_4\data02.txt');
m(maxS,2) = 0;
data(:,:,i) = m;
% 33: m4 a0 h1
i = i + 1;
m = load('ai_sonuc\alpha_yok_hash_var_4\data03.txt');
m(maxS,2) = 0;
data(:,:,i) = m;
% 34: m4 a0 h1
i = i + 1;
m = load('ai_sonuc\alpha_yok_hash_var_4\data04.txt');
m(maxS,2) = 0;
data(:,:,i) = m;
% 35: m4 a0 h1
i = i + 1;
m = load('ai_sonuc\alpha_yok_hash_var_4\data05.txt');
m(maxS,2) = 0;
data(:,:,i) = m;
% 36: m4 a0 h1
i = i + 1;
m = load('ai_sonuc\alpha_yok_hash_var_4\data06.txt');
m(maxS,2) = 0;
data(:,:,i) = m;
% 37: m4 a0 h1
i = i + 1;
m = load('ai_sonuc\alpha_yok_hash_var_4\data07.txt');
m(maxS,2) = 0;
data(:,:,i) = m;
% 38: m4 a0 h1
i = i + 1;
m = load('ai_sonuc\alpha_yok_hash_var_4\data08.txt');
m(maxS,2) = 0;
data(:,:,i) = m;
% 39: m4 a0 h1
i = i + 1;
m = load('ai_sonuc\alpha_yok_hash_var_4\data09.txt');
m(maxS,2) = 0;
data(:,:,i) = m;
% 40: m4 a0 h1
i = i + 1;
m = load('ai_sonuc\alpha_yok_hash_var_4\data10.txt');
m(maxS,2) = 0;
data(:,:,i) = m;
% 41: m4 a1 h1
i = i + 1;
m = load('ai_sonuc\alpha_var_hash_var_1\data01.txt');
m(maxS,2) = 0;
data(:,:,i) = m;
% 42: m4 a1 h1
i = i + 1;
m = load('ai_sonuc\alpha_var_hash_var_1\data02.txt');
m(maxS,2) = 0;
data(:,:,i) = m;
% 43: m4 a1 h1
i = i + 1;
m = load('ai_sonuc\alpha_var_hash_var_1\data03.txt');
m(maxS,2) = 0;
data(:,:,i) = m;
% 44: m4 a1 h1
i = i + 1;
m = load('ai_sonuc\alpha_var_hash_var_1\data04.txt');
m(maxS,2) = 0;
data(:,:,i) = m;
% 45: m4 a1 h1
i = i + 1;
m = load('ai_sonuc\alpha_var_hash_var_1\data05.txt');
m(maxS,2) = 0;
data(:,:,i) = m;

% alpha var hash yok
figure,
hold on
plot(squeeze(data(:,1,11:20)));
xlabel('turn number')
ylabel('execution time [ms]')
title({'Execution time vs. turn number';
    '(\alpha-\beta=on, hashing=off)'})
grid on

figure,
hold on
plot(squeeze(data(:,2,11:20)));
xlabel('turn number')
ylabel('number of nodes expanded')
title({'Number of nodes expanded vs. turn number';
    '(\alpha-\beta=on, hashing=off)'})
grid on

% alpha yok hash yok
figure,
hold on
plot(squeeze(data(:,1,(11:20)+10)));
xlabel('turn number')
ylabel('execution time [ms]')
title({'Execution time vs. turn number';
    '(\alpha-\beta=off, hashing=off)'})
grid on

figure,
hold on
plot(squeeze(data(:,2,(11:20)+10)));
xlabel('turn number')
ylabel('number of nodes expanded')
title({'Number of nodes expanded vs. turn number';
    '(\alpha-\beta=off, hashing=off)'})
grid on

% alpha yok hash var
figure,
hold on
plot(squeeze(data(:,1,([11:15, 17:20])+20)));
xlabel('turn number')
ylabel('execution time [ms]')
title({'Execution time vs. turn number';
    '(\alpha-\beta=off, hashing=on)'})
grid on
% legend('1','2','3','4','5','6','7','8','9','10');

figure,
hold on
plot(squeeze(data(:,2,(11:20)+20)));
xlabel('turn number')
ylabel('number of nodes expanded')
title({'Number of nodes expanded vs. turn number';
    '(\alpha-\beta=off, hashing=on)'})
grid on

% alpha var hash var
figure,
hold on
plot(squeeze(data(:,1,(11:15)+20)));
xlabel('turn number')
ylabel('execution time [ms]')
title({'Execution time vs. turn number';
    '(\alpha-\beta=on, hashing=on)'})
grid on

figure,
hold on
plot(squeeze(data(:,2,(11:15)+20)));
xlabel('turn number')
ylabel('number of nodes expanded')
title({'Number of nodes expanded vs. turn number';
    '(\alpha-\beta=on, hashing=on)'})
grid on


% karsilastirma
figure,
hold on
plot(squeeze(data(:,1,[11 21 31 41])));
xlabel('turn number')
ylabel('execution time [ms]')
title('Execution time vs. turn number')
legend(...
    'maxDepth=4, \alpha-\beta=on, hashing=off',...
    'maxDepth=4, \alpha-\beta=off, hashing=off',...
    'maxDepth=4, \alpha-\beta=off, hashing=on',...
    'maxDepth=4, \alpha-\beta=on, hashing=on');
grid on

figure,
hold on
plot(squeeze(data(:,2,[11 21 31 41])));
xlabel('turn number')
ylabel('number of nodes expanded')
title('Number of nodes expanded vs. turn number')
legend(...
    'maxDepth=4, \alpha-\beta=on, hashing=off',...
    'maxDepth=4, \alpha-\beta=off, hashing=off',...
    'maxDepth=4, \alpha-\beta=off, hashing=on',...
    'maxDepth=4, \alpha-\beta=on, hashing=on');
grid on

% farkli max depth'ler ile karsilastirma
figure,
hold on
plot(squeeze(data(:,1,[11 21 31 41])));
xlabel('turn number')
ylabel('execution time [ms]')
title('Execution time vs. turn number')
legend(...
    'maxDepth=4, \alpha-\beta=on, hashing=off',...
    'maxDepth=4, \alpha-\beta=off, hashing=off',...
    'maxDepth=4, \alpha-\beta=off, hashing=on',...
    'maxDepth=4, \alpha-\beta=on, hashing=on');
grid on

figure,
hold on
plot(squeeze(data(:,2,[11 21 31 41])));
xlabel('turn number')
ylabel('number of nodes expanded')
title('Number of nodes expanded vs. turn number')
legend(...
    'maxDepth=4, \alpha-\beta=on, hashing=off',...
    'maxDepth=4, \alpha-\beta=off, hashing=off',...
    'maxDepth=4, \alpha-\beta=off, hashing=on',...
    'maxDepth=4, \alpha-\beta=on, hashing=on');
grid on

return
figure,
hold on
plot(squeeze(data(:,1,[1 2 6 7])));
xlabel('turn number')
ylabel('execution time [ms]')
title('Execution time vs. turn number')
legend(...
    'maxDepth=4, \alpha-\beta=on, hashing=off',...
    'maxDepth=4, \alpha-\beta=off, hashing=off',...
    'maxDepth=4, \alpha-\beta=on, hashing=on',...
    'maxDepth=4, \alpha-\beta=off, hashing=on');
grid on

figure,
hold on
plot(squeeze(data(:,2,[1 2 6 7])));
xlabel('turn number')
ylabel('number of nodes expanded')
title('Number of nodes expanded vs. turn number')
legend(...
    'maxDepth=4, \alpha-\beta=on, hashing=off',...
    'maxDepth=4, \alpha-\beta=off, hashing=off',...
    'maxDepth=4, \alpha-\beta=on, hashing=on',...
    'maxDepth=4, \alpha-\beta=off, hashing=on');
grid on